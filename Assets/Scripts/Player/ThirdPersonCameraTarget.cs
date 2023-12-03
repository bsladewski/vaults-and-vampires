using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ThirdPersonCameraTarget : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 0.5f;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.ThirdPersonCamera.Zoom.started += ZoomCamera;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.ThirdPersonCamera.Zoom.started -= ZoomCamera;
    }

    private void LateUpdate()
    {
        float rotateInput = playerInput.ThirdPersonCamera.Rotate.ReadValue<float>();

        // multiplying by 360 so a rotation speed of 1 represents a full rotation in 1 second
        transform.Rotate(new Vector3(0f, rotateInput * rotateSpeed * 360f * Time.deltaTime, 0f));
    }

    private void ZoomCamera(InputAction.CallbackContext ctx)
    {
        ThirdPersonCameraController cameraController = CameraManager.Instance.GetThirdPersonCameraController();
        cameraController.CycleFollowOffset();
    }
}
