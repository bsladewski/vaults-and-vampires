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
            jumpInertia = planarMovement * jumpMoveSpeedModifier;
            currentVelocity += Vector3.up * jumpHeight * 100;
            shouldJump = false;
        }

        if (!isGrounded)
        {
            fallVelocity += gravity * Time.deltaTime;
        }

        currentVelocity += Vector3.down * fallVelocity;
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return layerMask == (layerMask | 1 << coll.gameObject.layer);
    }

    public void AfterCharacterUpdate(float deltaTime) { }

    public void BeforeCharacterUpdate(float deltaTime) { }

    public void OnDiscreteCollisionDetected(Collider hitCollider) { }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

    public void PostGroundingUpdate(float deltaTime)
    {
        if (IsGrounded())
        {
            fallVelocity = 0f;
        }
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
}
