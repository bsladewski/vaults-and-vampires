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
        playerInput.ThirdPersonMovement.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        playerInput.ThirdPersonMovement.Jump.performed -= Jump;
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
    }

    private Vector3 GetCameraForward()
    {
        return ThirdPersonCameraTarget.Instance.GetCameraForward();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (playerCharacterController.IsGrounded())
        {
            playerCharacterController.SetShouldJump();
        }
    }
}
