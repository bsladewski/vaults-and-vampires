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
    private float moveSpeed = 1f;

    [SerializeField]
    private float moveAcceleration = 1f;

    private Vector3 targetVelocityNormalized;

    [SerializeField]
    private float rotateSpeed = 1f;

    private Quaternion targetRotation;

    [SerializeField]
    private float gravity = 9.81f;

    private float fallVelocity;

    [SerializeField]
    private float jumpHeight = 1f;

    [SerializeField]
    private float jumpMoveSpeedModifier = 0.25f;

    private bool wasGrounded;

    private Vector3 jumpInertia;

    private bool shouldJump;

    private void Start()
    {
        motor.CharacterController = this;
    }

    public bool IsGrounded()
    {
        return motor.GroundingStatus.IsStableOnGround;
    }

    public void SetTargetVelocityNormalized(Vector3 targetVelocityNormalized)
    {
        this.targetVelocityNormalized = targetVelocityNormalized;
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
        if (targetVelocityNormalized != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(targetVelocityNormalized, Vector3.up);
        }

        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        bool isGrounded = IsGrounded();
        Vector3 planarMovement = targetVelocityNormalized * moveSpeed * (isGrounded ? 1f : jumpMoveSpeedModifier);
        if (!isGrounded)
        {
            planarMovement += jumpInertia;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, planarMovement, Time.deltaTime * moveAcceleration);
        if (shouldJump)
        {
            currentVelocity += Vector3.up * jumpHeight * 100; // TODO: remove magic number
            shouldJump = false;
        }

        if (!isGrounded)
        {
            fallVelocity += gravity * Time.deltaTime;
        }

        if (!isGrounded && wasGrounded)
        {
            jumpInertia = planarMovement * jumpMoveSpeedModifier;
        }

        currentVelocity += Vector3.down * fallVelocity;
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return layerMask == (layerMask | 1 << coll.gameObject.layer);
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        if (IsGrounded())
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
