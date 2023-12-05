using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraTarget : MonoBehaviour
{
    public static ThirdPersonCameraTarget Instance { get; private set; }

    [SerializeField]
    private ThirdPersonCameraController cameraController;

    [SerializeField]
    private float rotateSpeed = 0.5f;

    [SerializeField]
    private float yOffset = 0.75f;

    private Transform playerTransform;

    private PlayerInput playerInput;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton instance ThirdPersonCameraTarget already instantiated!");
        }
        Instance = this;

        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerInput.Enable();
        playerInput.ThirdPersonCamera.Zoom.started += ZoomCamera;
    }

    private void OnDisable()
    {
        playerInput.ThirdPersonCamera.Zoom.started -= ZoomCamera;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + Vector3.up * yOffset;

        if (playerInput.ThirdPersonCamera.Center.IsPressed())
        {
            transform.forward = playerTransform.forward;
        }
        else
        {
            float rotateInput = playerInput.ThirdPersonCamera.Rotate.ReadValue<float>();

            // multiplying by 360 so a rotation speed of 1 represents a full rotation in 1 second
            transform.Rotate(new Vector3(0f, rotateInput * rotateSpeed * 360f * Time.deltaTime, 0f));
        }
    }

    public Vector3 GetCameraForward()
    {
        return transform.forward;
    }

    private void ZoomCamera(InputAction.CallbackContext ctx)
    {
        cameraController.CycleFollowOffset();
    }
}
