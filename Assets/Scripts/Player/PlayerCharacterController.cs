using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

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
    private float jumpHeight = 1f;

    [SerializeField]
    private float jumpMoveSpeedModifier = 0.5f;

    private Vector3 jumpInertia;

    private bool shouldJump;

    private bool wasGrounded;

    private void Start()
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
            float jumpVelocity = Mathf.Sqrt(jumpHeight * 2 * gravity);
            currentVelocity.y += jumpVelocity;
            fallVelocity = -jumpVelocity;
            shouldJump = false;
        }

        if (!isGrounded)
        {
            // apply gravity
            fallVelocity += gravity * Time.deltaTime;
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
            jumpInertia = planarMovement * jumpMoveSpeedModifier;
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
