using Sirenix.OdinInspector;
using UnityEngine;
using Cameras;
using Utils;
using Events;

namespace Player
{
    public class MovementInputController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles movement of player kinematic rigidbody.")]
        [Required]
        [SerializeField]
        private MovementController movementController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Handles taking damage from damage sources.")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Tracks the abilities that the player has unlocked.")]
        [Required]
        [SerializeField]
        private AbilityController abilityController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Handles respawning the player after falling off the map or dying.")]
        [Required]
        [SerializeField]
        private RespawnController respawnController;

        [FoldoutGroup("Aim Locked Movement Settings")]
        [Tooltip("Adjusts player movement while strafing.")]
        [MinValue(0f)]
        [SerializeField]
        private float strafeMovementPenalty = 0.75f;

        [FoldoutGroup("Aim Locked Movement Settings")]
        [Tooltip("Adjusts player movement while moving backwards.")]
        [MinValue(0f)]
        [SerializeField]
        private float backwardsMovementPenalty = 0.5f;

        [FoldoutGroup("Aim Locked Movement Settings")]
        [Tooltip("Determines how quickly the camera should adjust its rotation while aim locked.")]
        [MinValue(0f)]
        [SerializeField]
        private float cameraCorrectionSpeed = 5f;

        [FoldoutGroup("Other Settings")]
        [Tooltip("A grace period in seconds after becoming ungrounded where the player can jump.")]
        [SerializeField]
        private float coyoteTime = 0.15f;

        [FoldoutGroup("Other Settings")]
        [Tooltip("A grace period in seconds before becoming grounded where the player can jump.")]
        [SerializeField]
        private float jumpBuffer = 0.15f;

        [FoldoutGroup("Other Settings")]
        [Tooltip("The time in seconds after taking fall damage before the player can move again.")]
        [SerializeField]
        private float hardLandingCooldown = 0.2f;

        private float coyoteTimeTimer;

        private float jumpBufferTimer;

        private float hardLandingTimer;

        private PlayerInput playerInput;

        private Vector3 lastCameraForward;

        private Vector3 movementDirection;

        private bool startedJump;

        private bool isMovementLocked;

        private bool isDoubleJumpValid;

        private bool hasDoubleJumped;

        private bool startedDoubleJump;

        private float aimLockRotation;

        private void Awake()
        {
            playerInput = new PlayerInput();
        }

        private void Start()
        {
            lastCameraForward = GetCameraForward();
        }

        private void OnEnable()
        {
            playerInput.Enable();

            movementController.OnJumped += OnJumped;
            movementController.OnFell += OnFell;
            movementController.OnLanded += OnLanded;
            movementController.OnHardLanding += OnHardLanding;
            healthManager.OnDeath += OnDeath;
            respawnController.OnRespawn += OnRespawn;

            EventsSystem.Instance.abilityEvents.OnSpellOrbTriggered += OnSpellOrbTriggered;
        }

        private void OnDisable()
        {
            movementController.OnJumped -= OnJumped;
            movementController.OnFell -= OnFell;
            movementController.OnLanded -= OnLanded;
            movementController.OnHardLanding -= OnHardLanding;
            healthManager.OnDeath -= OnDeath;
            respawnController.OnRespawn -= OnRespawn;

            EventsSystem.Instance.abilityEvents.OnSpellOrbTriggered -= OnSpellOrbTriggered;
        }

        private void Update()
        {
            if (hardLandingTimer > 0f)
            {
                hardLandingTimer -= Time.deltaTime;
            }
            else if (!isMovementLocked)
            {
                HandleMovementInput();
                HandleJumpInput();
            }
        }

        public bool GetIsAimLocked()
        {
            return ThirdPersonCameraTarget.Instance.GetIsAimLocked();
        }

        public Vector3 GetMovementDirection()
        {
            return movementDirection;
        }

        public float GetAimLockRotation()
        {
            return aimLockRotation;
        }

        private void HandleMovementInput()
        {
            // get player movement input
            Vector2 runInput = playerInput.ThirdPersonMovement.Run.ReadValue<Vector2>();
            movementDirection = new Vector3(runInput.x, 0f, runInput.y);
            bool isAimLocked = GetIsAimLocked();
            if (isAimLocked && runInput.y < 0f)
            {
                // if we're aim locked slow down the player's backwards movement
                runInput.y *= backwardsMovementPenalty;
            }

            if (isAimLocked)
            {
                // if we're aim locked slow down the player's sideways movement
                runInput.x *= strafeMovementPenalty;

                // when we're aim locked we replace camera rotation with player rotation
                aimLockRotation = playerInput.ThirdPersonMovement.Rotate.ReadValue<float>();
                movementController.SetAimLockRotation(aimLockRotation);
            }
            else
            {
                // if we're not aim locked, clear aim lock rotation
                movementController.SetAimLockRotation(0f);
            }

            Vector3 runDirection = new Vector3(runInput.x, 0f, runInput.y);
            if (runDirection == Vector3.zero)
            {
                // if the player is not trying to move, align player movement with camera
                lastCameraForward = GetCameraForward();
            }
            else
            {
                // if the player is trying to move, interpolate player movement to align with camera
                lastCameraForward = Vector3.Slerp(
                    lastCameraForward,
                    GetCameraForward(),
                    Time.deltaTime * cameraCorrectionSpeed
                );
            }

            // calculate player's intended velocity and update movement controller
            Vector3 targetForward = isAimLocked ? transform.forward : lastCameraForward;
            Vector3 targetDirection = Quaternion.LookRotation(targetForward) * runDirection;
            movementController.SetTargetDirection(targetDirection);
        }

        private void HandleJumpInput()
        {
            // get player jump input
            bool isGrounded = movementController.GetIsGrounded();
            bool jumpInput = playerInput.ThirdPersonMovement.Jump.triggered;
            if ((jumpInput && isGrounded) || (jumpInput && coyoteTimeTimer > 0f) || (isGrounded && jumpBufferTimer > 0f))
            {
                // if the player presses jump while grounded, initiate a jump
                // coyote time honors player jump input slightly after becoming ungrounded
                // jump buffer honors player jump input slightly before becoming grounded
                movementController.SetShouldJump(true);
                movementController.ResetFallVelocity();
                coyoteTimeTimer = 0f;
                jumpBufferTimer = 0f;
                startedJump = true;
            }
            else if (abilityController.GetCanDoubleJump() && isDoubleJumpValid && !hasDoubleJumped && jumpInput)
            {
                // if the player pressed jump and they are able to double jump, initiate a double jump
                TriggerDoubleJump(false);
            }
            else if (playerInput.ThirdPersonMovement.Jump.IsPressed())
            {
                // if the player didn't just jump and is pressing the jump button, set is jump held flag
                movementController.SetIsJumpHeld(true);
            }
            else
            {
                // if the player didn't just jump and is not pressing the jump button, reset is jump held flag
                movementController.SetIsJumpHeld(false);
            }

            if (!isGrounded)
            {
                // the player may remain grounded for a brief time after the player started jumping, we want to disable
                // coyote time until we've actually left the ground
                // once we have left the ground we can clear this flag so coyote time works as intended
                startedJump = false;
            }

            if (isGrounded && !startedJump)
            {
                // if we're grounded and aren't trying to jump, reset coyote time
                coyoteTimeTimer = coyoteTime;
            }
            else if (coyoteTimeTimer > 0f)
            {
                // if we're not grounded and and the player is not trying to jump, burn down coyote time
                coyoteTimeTimer -= Time.deltaTime;
            }

            if (jumpInput && !isGrounded && !startedDoubleJump)
            {
                // if the player tried to jump, we're not grounded, and we didn't just double jump,
                // reset the jump buffer
                jumpBufferTimer = jumpBuffer;
            }
            else if (jumpBufferTimer > 0f)
            {
                // if the player didn't try to jump or the player is grounded, burn down the jump buffer
                jumpBufferTimer -= Time.deltaTime;
            }

            startedDoubleJump = false;
        }

        private Vector3 GetCameraForward()
        {
            return ThirdPersonCameraTarget.Instance.GetCameraForward();
        }

        private void OnJumped()
        {
            // when the player jumps, set the flag to allow them to double jump
            isDoubleJumpValid = true;
        }

        private void OnFell()
        {
            // when the player falls, set the flag to allow them to double jump
            isDoubleJumpValid = true;
        }

        private void OnLanded()
        {
            // when the player lands, reset the flags for double jumping
            isDoubleJumpValid = false;
            hasDoubleJumped = false;
        }

        private void OnHardLanding()
        {
            // if the player takes fall damage, temporarily block movement input
            hardLandingTimer = hardLandingCooldown;
            ResetMovementInput();
        }

        private void OnDeath()
        {
            // if the player dies, temporarily block movement input
            isMovementLocked = true;
            ResetMovementInput();
        }

        private void OnRespawn()
        {
            // if the player respawns, remove the temporary block on movement input
            isMovementLocked = false;
        }

        private void OnSpellOrbTriggered(GameObject target, SpellType spellType)
        {
            // if this game object consumed a double jump spell orb, trigger a double jump
            if (target == gameObject && spellType == SpellType.DoubleJump)
            {
                TriggerDoubleJump(true);
            }
        }

        private void ResetMovementInput()
        {
            // reset any ongoing movement input
            movementDirection = Vector3.zero;
            movementController.SetTargetDirection(Vector3.zero);
            movementController.SetShouldJump(false);
            movementController.SetShouldDoubleJump(false, false);
            movementController.SetIsJumpHeld(false);
        }

        private void TriggerDoubleJump(bool fromEnvironmentTrigger)
        {
            movementController.SetShouldDoubleJump(true, fromEnvironmentTrigger);
            movementController.ResetFallVelocity();
            if (!fromEnvironmentTrigger)
            {
                hasDoubleJumped = true;
            }
            startedDoubleJump = true;
        }
    }
}
