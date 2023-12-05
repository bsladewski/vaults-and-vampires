using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    [Required]
    [SerializeField]
    private PlayerCharacterController playerCharacterController;

    [SerializeField]
    private float cameraCorrectionSpeed = 5f;


    [SerializeField]
    private float coyoteTime = 0.2f;

    private float coyoteTimeTimer;

    private bool isJumping;

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
        playerInput.ThirdPersonMovement.Jump.started += Jump;
    }

    private void OnDisable()
    {
        playerInput.ThirdPersonMovement.Jump.started -= Jump;
    }

    private void Update()
    {
        Vector2 runInput = playerInput.ThirdPersonMovement.Run.ReadValue<Vector2>();
        Vector3 runDirection = new Vector3(runInput.x, 0f, runInput.y);
        if (runDirection == Vector3.zero)
        {
            lastCameraForward = GetCameraForward();
        }
        else
        {
            lastCameraForward = Vector3.Slerp(
                lastCameraForward,
                GetCameraForward(),
                Time.deltaTime * cameraCorrectionSpeed
            );
        }

        Vector3 targetVelocityNormalized = (Quaternion.LookRotation(lastCameraForward) * runDirection).normalized;
        playerCharacterController.SetTargetVelocityNormalized(targetVelocityNormalized);

        bool isGrounded = playerCharacterController.IsGrounded();
        if (!isJumping && isGrounded)
        {
            coyoteTimeTimer = coyoteTime;
        }
        else
        {
            coyoteTimeTimer -= Time.deltaTime;
        }

        if (!isGrounded)
        {
            isJumping = false;
        }
    }

    private Vector3 GetCameraForward()
    {
        return ThirdPersonCameraTarget.Instance.GetCameraForward();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (coyoteTimeTimer > 0f || playerCharacterController.IsGrounded())
        {
            playerCharacterController.SetShouldJump();
            isJumping = true;
            coyoteTimeTimer = 0f;
        }
    }
}
