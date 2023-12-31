using Sirenix.OdinInspector;
using UnityEngine;

public class MovementInputController : MonoBehaviour
{
    [Required]
    [SerializeField]
    private MovementController movementController;

    [SerializeField]
    private float strafeMovementPenalty = 0.75f;

    [SerializeField]
    private float backwardsMovementPenalty = 0.5f;

    private PlayerInput playerInput;

    [SerializeField]
    private float cameraCorrectionSpeed = 5f;

    [SerializeField]
    private float coyoteTime = 0.15f;

    private float coyoteTimeTimer;

    [SerializeField]
    private float jumpBuffer = 0.15f;

    private float jumpBufferTimer;

    private bool startedJump;

    [SerializeField]
    private float hardLandingCooldown = 0.2f;

    private float hardLandingTimer;

    private Vector3 lastCameraForward;

    private Vector3 movementDirection;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        lastCameraForward = GetCameraForward();
        movementController.OnHardLanding += OnHardLanding;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Update()
    {
        if (hardLandingTimer > 0f)
        {
            hardLandingTimer -= Time.deltaTime;
        }
        else
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
            float aimLockRotation = playerInput.ThirdPersonMovement.Rotate.ReadValue<float>();
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

        // calculate player's intended velocity
        Vector3 targetDirection = Quaternion.LookRotation(lastCameraForward) * runDirection;
        movementController.SetTargetDirection(targetDirection);
    }

    private void HandleJumpInput()
    {
        bool isGrounded = movementController.GetIsGrounded();
        bool jumpInput = playerInput.ThirdPersonMovement.Jump.triggered;
        if ((jumpInput && isGrounded) || (jumpInput && coyoteTimeTimer > 0f) || (isGrounded && jumpBufferTimer > 0f))
        {
            movementController.SetShouldJump();
            coyoteTimeTimer = 0f;
            jumpBufferTimer = 0f;
            startedJump = true;
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

        if (jumpInput && !isGrounded)
        {
            // if the player tried to jump and we're not grounded, reset the jump buffer
            jumpBufferTimer = jumpBuffer;
        }
        else if (jumpBufferTimer > 0f)
        {
            // if the player didn't try to jump or the player is grounded, burn down the jump buffer
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    private Vector3 GetCameraForward()
    {
        return ThirdPersonCameraTarget.Instance.GetCameraForward();
    }

    private void OnHardLanding()
    {
        hardLandingTimer = hardLandingCooldown;
        ResetMovementInput();
    }

    private void ResetMovementInput()
    {
        movementDirection = Vector3.zero;
        movementController.SetTargetDirection(Vector3.zero);
        movementController.SetShouldJump(false);
        movementController.SetIsJumpHeld(false);
    }
}
