using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using Cameras;
using Utils;

namespace Player
{
    [SelectionBase]
    public class MovementController : MonoBehaviour, ICharacterController
    {
        public Action OnJumped;

        public Action OnFell;

        public Action OnLanded;

        public Action OnHardLanding;

        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private KinematicCharacterMotor motor;

        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [Required]
        [SerializeField]
        private DamageReceiver damageReceiver;

        [Header("Movement Settings")]
        [SerializeField]
        private float moveSpeed = 8f;

        private Vector3 targetDirection;

        private Vector3 planarMovement;

        [SerializeField]
        private float moveAcceleration = 10f;

        [SerializeField]
        private float rotateSpeed = 10f;

        [Header("Jump Settings")]
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

        private bool wasGrounded = true;

        [Header("Fall Settings")]
        [SerializeField]
        private float gravity = 9.81f;

        private float fallVelocity;

        [SerializeField]
        private float hardLandingVelocity = 15f;

        private bool wasHardLanding;

        [Header("Other Settings")]
        [SerializeField]
        private LayerMask collisionLayerMask;

        [SerializeField]
        private float aimLockRotateSpeed;

        private float aimLockRotation = 0.25f;

        [SerializeField]
        private float lateralKnockbackSpeed = 50f;

        [SerializeField]
        private float maxKnockbackHeight = 1f;

        private bool shouldResetVelocity;

        private Vector3 knockbackDirection;

        private float knockbackIntensity;

        private bool wasKnockedBack;

        private bool knockbackMovementLock;

        private void Awake()
        {
            motor.CharacterController = this;
        }

        private void OnEnable()
        {
            damageReceiver.OnKnockback += OnKnockback;
        }

        private void OnDisable()
        {
            damageReceiver.OnKnockback -= OnKnockback;
        }

        public Vector3 GetPlanarMovementNormalized()
        {
            return planarMovement / moveSpeed;
        }

        public bool GetIsGrounded()
        {
            return motor.GroundingStatus.IsStableOnGround;
        }

        public bool GetWasHardLanding()
        {
            return wasHardLanding;
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
            SetShouldJump(true);
        }

        public void SetShouldJump(bool shouldJump)
        {
            this.shouldJump = shouldJump;
        }

        public void SetIsJumpHeld(bool isJumpHeld)
        {
            this.isJumpHeld = isJumpHeld;
        }

        public void SetPosition(Vector3 position)
        {
            motor.SetPosition(position);
        }

        public void ResetFallVelocity()
        {
            fallVelocity = 0f;
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
            if (shouldResetVelocity)
            {
                currentVelocity = Vector3.zero;
                shouldResetVelocity = false;
                return;
            }

            if (knockbackIntensity > 0f)
            {
                // calculate lateral knockback
                Vector3 currentLateralVelocity = new Vector3(
                    currentVelocity.x,
                    0f,
                    currentVelocity.z
                );
                Vector3 lateralKnockback = new Vector3(
                    knockbackDirection.x,
                    0f,
                    knockbackDirection.z
                );

                lateralKnockback *= Vector3.Dot(-currentLateralVelocity, lateralKnockback);
                lateralKnockback = (lateralKnockback - currentLateralVelocity).normalized;

                lateralKnockback *= lateralKnockbackSpeed * knockbackIntensity;
                currentVelocity = lateralKnockback;

                // calculate vertical knockback
                motor.ForceUnground();
                fallVelocity -= Mathf.Sqrt(2f * maxKnockbackHeight * gravity * 2f);

                jumpInertia = lateralKnockback;
                wasKnockedBack = true;
                knockbackMovementLock = true;

                // reset knockback
                knockbackDirection = Vector3.zero;
                knockbackIntensity = 0f;
                return;
            }

            Vector3 initialVelocity = currentVelocity;

            bool isGrounded = GetIsGrounded();
            targetDirection = knockbackMovementLock ? Vector3.zero : targetDirection;
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
                fallVelocity -= Mathf.Sqrt(2f * maxJumpHeight * gravity);
                shouldJump = false;
                StartCoroutine(FireJumpEvent());
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

            if (initialVelocity.y >= 0f && currentVelocity.y < 0f)
            {
                StartCoroutine(FireFallEvent());
            }
        }

        public bool IsColliderValidForCollisions(Collider collider)
        {
            return CollisionUtils.IsColliderInLayerMask(collider, collisionLayerMask);
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            bool isGrounded = GetIsGrounded();
            if (!isGrounded && wasGrounded && !wasKnockedBack)
            {
                // if we left the ground, store our lateral momentum
                jumpInertia = planarMovement * (1f - jumpMoveSpeedModifier);
            }

            if (isGrounded)
            {
                if (!wasGrounded)
                {
                    knockbackMovementLock = false;
                    wasHardLanding = fallVelocity >= hardLandingVelocity;
                    if (wasHardLanding)
                    {
                        shouldResetVelocity = true;
                    }
                    StartCoroutine(FireLandEvent());
                }

                wasGrounded = true;
                jumpInertia = Vector3.zero;
                fallVelocity = 0f;
            }
            else
            {
                wasGrounded = false;
            }

            wasKnockedBack = false;
        }

        private IEnumerator FireJumpEvent()
        {
            yield return new WaitForEndOfFrame();
            OnJumped?.Invoke();
        }

        private IEnumerator FireFallEvent()
        {
            yield return new WaitForEndOfFrame();
            OnFell?.Invoke();
        }

        private IEnumerator FireLandEvent()
        {
            yield return new WaitForEndOfFrame();
            OnLanded?.Invoke();
            if (wasHardLanding)
            {
                OnHardLanding?.Invoke();
                healthManager.UpdateHealth(-1);
            }
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            if (!healthManager.IsDead() && !damageReceiver.IsInvulnerable() && hitCollider.gameObject.tag == "Ground Hazard")
            {
                DamageSource damageSource = hitCollider.gameObject.GetComponent<DamageSource>();
                if (damageSource != null)
                {
                    damageReceiver.TakeDamage(damageSource);
                }
                else
                {
                    Debug.LogError("Ground hazard is missing damage source!");
                }
            }
        }

        private void OnKnockback(Vector3 direction, float intensity)
        {
            knockbackDirection = direction;
            knockbackIntensity = intensity;
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

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // Not needed at this time
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            // Not needed at this time
        }
    }
}
