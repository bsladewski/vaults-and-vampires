using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class ThirdPersonCameraTarget : MonoBehaviour
{
    public static ThirdPersonCameraTarget Instance { get; private set; }

    [SerializeField]
    private ThirdPersonCameraController cameraController;

    [SerializeField]
    private float rotateSpeed = 0.5f;

    [SerializeField]
    private float yOffset = 0.75f;

    [SerializeField]
    private float lookSpeed = 0.6f;

    [SerializeField]
    private Ease lookEase = Ease.OutQuad;

    private Tween lookTween;

    private Transform playerTransform;

    private PlayerInput playerInput;

    private bool isAimLocked;

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
        playerInput.ThirdPersonCamera.Aim.performed += AimCamera;
    }

    private void OnDisable()
    {
        playerInput.ThirdPersonCamera.Zoom.started -= ZoomCamera;
        playerInput.ThirdPersonCamera.Aim.performed -= AimCamera;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + Vector3.up * yOffset;

        if (!playerInput.ThirdPersonCamera.Aim.IsPressed())
        {
            // if the player is not holding down the Aim button, reset the aim lock
            isAimLocked = false;

            if (lookTween == null || !lookTween.active)
            {
                // if the aim is not locked and we're not looking, allow the player to rotate the camera
                float rotateInput = playerInput.ThirdPersonCamera.Rotate.ReadValue<float>();

                // multiplying by 360 so a rotation speed of 1 represents a full rotation in 1 second
                transform.Rotate(new Vector3(0f, rotateInput * rotateSpeed * 360f * Time.deltaTime, 0f));
            }
        }

        if (isAimLocked && (lookTween == null || !lookTween.active))
        {
            // if we're aim locked and we're not looking keep the camera pointed towards the player forward
            transform.forward = playerTransform.forward;
        }
    }

    public Vector3 GetCameraForward()
    {
        return transform.forward;
    }

    public bool GetIsAimLocked()
    {
        return isAimLocked;
    }

    private void ZoomCamera(InputAction.CallbackContext ctx)
    {
        cameraController.CycleFollowOffset();
    }

    private void AimCamera(InputAction.CallbackContext ctx)
    {
        float lookDuration = Vector3.Angle(transform.forward, playerTransform.forward) / 180f * lookSpeed;
        lookTween = DOTween.To(
            () => transform.forward,
            value => transform.forward = value,
            playerTransform.forward,
            lookDuration
        ).SetEase(lookEase);
        isAimLocked = true;
    }
}
