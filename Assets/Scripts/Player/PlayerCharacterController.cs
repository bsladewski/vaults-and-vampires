using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using System;

[SelectionBase]
public class PlayerCharacterController : MonoBehaviour, ICharacterController
{
    public Action OnPlayerJumped;

    public Action OnPlayerLanded;

    [Required]
    [SerializeField]
    private KinematicCharacterMotor motor;

    [SerializeField]
    private LayerMask collisionLayerMask;

    [SerializeField]
    private float moveSpeed = 8f;

    private Vector3 targetDirection;

    private Vector3 planarMovement;

    [SerializeField]
    private float moveAcceleration = 10f;

    [SerializeField]
    private float rotateSpeed = 10f;

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

    [SerializeField]
    private float jumpRotationSpeedModifier = 0.25f;

    private Vector3 jumpInertia;

    private bool wasGrounded;

    [SerializeField]
    private float aimLockRotateSpeed;

    private float aimLockRotation = 0.25f;

    private void Awake()
    {
        motor.CharacterController = this;
    }

    public float GetSpeedNormalized()
    {
        return (planarMovement / moveSpeed).magnitude;
    }

    public bool GetIsGrounded()
    {
        return motor.GroundingStatus.IsStableOnGround;
    }

    public void SetTargetDirection(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void SetAimLockRotation(float aimLockRotation)
    {
        this.aimLockRotation = aimLockRotation;
    }

    public void SetShouldJump()
    {
        shouldJump = true;
    }

    public void SetIsJumpHeld(bool isJumpHeld)
    {
        this.isJumpHeld = isJumpHeld;
    }

    public void SetPosition(Vector3 position)
    {
        motor.SetPosition(position);
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (ThirdPersonCameraTarget.Instance.GetIsAimLocked())
        {
            float targetAimLockRotation = aimLockRotation * aimLockRotateSpeed * 360f;
            currentRotation *= Quaternion.Euler(Vector3.up * targetAimLockRotation * Time.deltaTime);
            return;
        }

        if (targetDirection != Vector3.zero)
        {
            float rotationSpeedModifier = GetIsGrounded() ? rotateSpeed : rotateSpeed * jumpRotationSpeedModifier;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeedModifier);
        }

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
            OnPlayerJumped?.Invoke();
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

    public bool IsColliderValidForCollisions(Collider collider)
    {
        return CollisionUtils.IsColliderInLayerMask(collider, collisionLayerMask);
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
            if (!wasGrounded)
            {
                OnPlayerLanded?.Invoke();
            }

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
