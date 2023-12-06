using Sirenix.OdinInspector;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Required]
    [SerializeField]
    private PlayerCharacterController playerCharacterController;

    [SerializeField]
    private float cameraCorrectionSpeed = 5f;

    [SerializeField]
    private float coyoteTime = 0.15f;

    private float coyoteTimeTimer;

    [SerializeField]
    private float jumpBuffer = 0.15f;

    private float jumpBufferTimer;

    private bool startedJump;

    private PlayerInput playerInput;

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

    private void LateUpdate()
    {
        // get player movement input
        Vector2 runInput = playerInput.ThirdPersonMovement.Run.ReadValue<Vector2>();
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
        Vector3 targetVelocityNormalized = (Quaternion.LookRotation(lastCameraForward) * runDirection).normalized;
        playerCharacterController.SetTargetVelocityNormalized(targetVelocityNormalized);

        bool isGrounded = playerCharacterController.GetIsGrounded();
        bool jumpInput = playerInput.ThirdPersonMovement.Jump.triggered;
        if ((jumpInput && isGrounded) || (jumpInput && coyoteTimeTimer > 0f) || (isGrounded && jumpBufferTimer > 0f))
        {
            playerCharacterController.SetShouldJump();
            coyoteTimeTimer = 0f;
            jumpBufferTimer = 0f;
            startedJump = true;
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
}
