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

        public Action OnDoubleJumped;

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

        [SerializeField]
        private float moveAcceleration = 10f;

        [SerializeField]
        private float rotateSpeed = 10f;

        private Vector3 targetDirection;

        private Vector3 planarMovement;

        [Header("Jump Settings")]
        [SerializeField]
        private float minJumpHeight = 1f;

        [SerializeField]
        private float maxJumpHeight = 2f;

        [SerializeField]
        private float jumpMoveSpeedModifier = 0.5f;

        [SerializeField]
        private float jumpRotateSpeedModifier = 0.25f;

        private Vector3 jumpInertia;

        private bool shouldJump;

        private bool shouldDoubleJump;

        private bool isJumpHeld;

        private bool ensureMaxJumpHeight;

        private bool ignoreJumpMoveSpeedModifier;

        private bool wasGrounded = true;

        [Header("Fall Settings")]
        [SerializeField]
        private float gravity = 9.81f;

        [SerializeField]
        private float hardLandingVelocity = 15f;

        private float fallVelocity;

        private bool wasHardLanding;

        [Header("Other Settings")]
        [SerializeField]
        private LayerMask collisionLayerMask;

        [SerializeField]
        private float aimLockRotateSpeed;

        [SerializeField]
        private float lateralKnockbackSpeed = 50f;

        [SerializeField]
        private float maxKnockbackHeight = 1f;

        private float aimLockRotation = 0.25f;

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

        public void SetShouldJump(bool shouldJump)
        {
            this.shouldJump = shouldJump;
        }

        public void SetShouldDoubleJump(bool shouldDoubleJump, bool fromEnvironmentTrigger)
        {
            if (fromEnvironmentTrigger)
            {
                // this removes some input driven double jump mechanics to make environmental triggers easier to use
                ensureMaxJumpHeight = true;
                ignoreJumpMoveSpeedModifier = true;
            }
            else
            {
                ensureMaxJumpHeight = false;
                ignoreJumpMoveSpeedModifier = false;
            }

            this.shouldDoubleJump = shouldDoubleJump;
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
                // if the player is aim locked, rotation from input should be directly applied to the player rather than
                // being derived from movement
                float targetAimLockRotation = aimLockRotation * aimLockRotateSpeed * 360f;
                currentRotation *= Quaternion.Euler(Vector3.up * targetAimLockRotation * Time.deltaTime);
                return;
            }

            if (targetDirection != Vector3.zero)
            {
                // calculate player rotation based on player movement
                float rotationSpeedModifier = GetIsGrounded() ? rotateSpeed : rotateSpeed * jumpRotateSpeedModifier;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeedModifier);
            }

        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (shouldResetVelocity)
            {
                // if the flag to reset velocity is set, zero out velocity and return early
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
            Vector3 basePlanarMovement = targetDirection * moveSpeed;
            planarMovement = basePlanarMovement * (isGrounded || ignoreJumpMoveSpeedModifier ? 1f : jumpMoveSpeedModifier);
            if (!isGrounded)
            {
                // conserve momentum if we are jumping
                planarMovement += jumpInertia;
            }

            currentVelocity = Vector3.Lerp(currentVelocity, planarMovement, Time.deltaTime * moveAcceleration);

            if (shouldJump || shouldDoubleJump)
            {
                // initiate a jump
                motor.ForceUnground();

                // apply velocity to reach max jump height
                fallVelocity -= Mathf.Sqrt(2f * maxJumpHeight * gravity);
                if (shouldJump)
                {
                    StartCoroutine(FireJumpEvent());
                }
                else
                {
                    if (ignoreJumpMoveSpeedModifier)
                    {
                        jumpInertia = Vector3.zero;
                    }
                    else
                    {
                        jumpInertia = basePlanarMovement * (1f - jumpMoveSpeedModifier);
                    }
                    StartCoroutine(FireDoubleJumpEvent());
                }

                shouldJump = false;
                shouldDoubleJump = false;
            }
            else if (!isGrounded)
            {
                if (!isJumpHeld && !ensureMaxJumpHeight && fallVelocity < 0f)
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
                // if the player's velocity was zero or positive but has become negative, fire an event to indicate that
                // the player has begun falling
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
                    // if the player has just become grounded, reset jump/fall related flags
                    knockbackMovementLock = false;
                    ensureMaxJumpHeight = false;
                    ignoreJumpMoveSpeedModifier = false;

                    wasHardLanding = fallVelocity >= hardLandingVelocity;
                    if (wasHardLanding)
                    {
                        // if the player takes fall damage, reset their velocity on the next frame
                        // this is because we temporarily block movement input after a hard landing
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

        private IEnumerator FireDoubleJumpEvent()
        {
            yield return new WaitForEndOfFrame();
            OnDoubleJumped?.Invoke();
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
                // if we land on a ground hazard and the player can take damage, damage the player
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
            // set the values needed to calculate knockback movement
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
