using Sirenix.OdinInspector;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Required]
    [SerializeField]
    private PlayerCharacterController playerCharacterController;

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


    private Vector3 lastCameraForward;

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
    }

    private void Update()
    {
        // get player movement input
        Vector2 runInput = playerInput.ThirdPersonMovement.Run.ReadValue<Vector2>();
        if (GetIsAimLocked() && runInput.y < 0f)
        {
            // if we're aim locked slow down the player's backwards movement
            runInput.y *= backwardsMovementPenalty;
        }
        if (GetIsAimLocked())
        {
            // if we're aim locked slow down the player's sideways movement
            runInput.x *= strafeMovementPenalty;
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
        playerCharacterController.SetTargetDirection(targetDirection);

        bool isGrounded = playerCharacterController.GetIsGrounded();
        bool jumpInput = playerInput.ThirdPersonMovement.Jump.triggered;
        if ((jumpInput && isGrounded) || (jumpInput && coyoteTimeTimer > 0f) || (isGrounded && jumpBufferTimer > 0f))
        {
            playerCharacterController.SetShouldJump();
            coyoteTimeTimer = 0f;
            jumpBufferTimer = 0f;
            startedJump = true;
        }
        else if (playerInput.ThirdPersonMovement.Jump.IsPressed())
        {
            // if the player didn't just jump and is pressing the jump button, set is jump held flag
            playerCharacterController.SetIsJumpHeld(true);
        }
        else
        {
            // if the player didn't just jump and is not pressing the jump button, reset is jump held flag
            playerCharacterController.SetIsJumpHeld(false);
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

    private bool GetIsAimLocked()
    {
        return ThirdPersonCameraTarget.Instance.GetIsAimLocked();
    }
}
