using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;

public class PlayerCharacterController : MonoBehaviour, ICharacterController
{
    [Required]
    [SerializeField]
    private KinematicCharacterMotor motor;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float moveSpeed = 8f;

    private Vector3 targetDirection;

    private Vector3 planarMovement;

    [SerializeField]
    private float moveAcceleration = 10f;


    [SerializeField]
    private float rotateSpeed = 10f;

    private Quaternion targetRotation;

    [SerializeField]
    private float gravity = 9.81f;

    private float fallVelocity;

    [SerializeField]
    private float minJumpHeight = 1f;

    [SerializeField]
    private float maxJumpHeight = 2f;

    private bool shouldJump;

    private bool isJumpHeld;

    [SerializeField]
    private float jumpMoveSpeedModifier = 0.5f;

    private Vector3 jumpInertia;

    private bool wasGrounded;

    private void Awake()
    {
        motor.CharacterController = this;
    }

    public bool GetIsGrounded()
    {
        return motor.GroundingStatus.IsStableOnGround;
    }

    public void SetTargetDirection(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void SetTargetRotation(Quaternion targetRotation)
    {
        this.targetRotation = targetRotation;
    }

    public void SetShouldJump()
    {
        shouldJump = true;
    }

    public void SetIsJumpHeld(bool isJumpHeld)
    {
        this.isJumpHeld = isJumpHeld;
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (ThirdPersonCameraTarget.Instance.GetIsAimLocked())
        {
            // if we're aim locked the player should remain facing their current forward vector
            return;
        }

        if (targetDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        }

        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        bool isGrounded = GetIsGrounded();
        planarMovement = targetDirection * moveSpeed * (isGrounded ? 1f : jumpMoveSpeedModifier);
        if (!isGrounded)
        {
            // conserve momentum if we are jumping
            planarMovement += jumpInertia;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, planarMovement, Time.deltaTime * moveAcceleration);

        if (shouldJump)
        {
            // initiate a jump
            motor.ForceUnground();

            // apply velocity to reach max jump height
            fallVelocity -= Mathf.Sqrt(2 * maxJumpHeight * gravity);
            shouldJump = false;
        }

        else if (!isGrounded)
        {
            if (!isJumpHeld && fallVelocity < 0f)
            {
                // if the player releases the jump button, adjust gravity so we hit the target jump height
                fallVelocity += gravity * (maxJumpHeight / minJumpHeight) * Time.deltaTime;
            }
            else
            {
                // calculate gravity
                fallVelocity += gravity * Time.deltaTime;
            }

            // apply gravity
            currentVelocity.y = -fallVelocity;
        }
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return layerMask == (layerMask | 1 << coll.gameObject.layer);
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        bool isGrounded = GetIsGrounded();
        if (!isGrounded && wasGrounded)
        {
            // if we left the ground, store our lateral momentum
            jumpInertia = planarMovement * (1f - jumpMoveSpeedModifier);
        }

        if (isGrounded)
        {
            wasGrounded = true;
            jumpInertia = Vector3.zero;
            fallVelocity = 0f;
        }
        else
        {
            wasGrounded = false;
        }
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        // Not needed at this time
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        // Not needed at this time
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        // Not needed at this time
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // Not needed at this time
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // Not needed at this time
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        // Not needed at this time
    }
}
