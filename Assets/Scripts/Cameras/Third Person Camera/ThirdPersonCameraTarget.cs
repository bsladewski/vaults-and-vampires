using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras
{
    public class ThirdPersonCameraTarget : MonoBehaviour
    {
        public static ThirdPersonCameraTarget Instance { get; private set; }

        [Header("Dependencies")]
        [SerializeField]
        private ThirdPersonCameraController cameraController;

        [Header("Settings")]
        [SerializeField]
        private float rotateSpeed = 0.5f;

        [SerializeField]
        private int fixedRotateIncrement = 45;

        [SerializeField]
        private float aimSpeed = 15f;

        [SerializeField]
        private float yOffset = 0.75f;

        private bool isAimLocked;

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
            playerInput.ThirdPersonCamera.FixedRotate.started += FixedRotate;
        }

        private void OnDisable()
        {
            playerInput.ThirdPersonCamera.Zoom.started -= ZoomCamera;
            playerInput.ThirdPersonCamera.FixedRotate.started -= FixedRotate;
        }

        private void LateUpdate()
        {
            transform.position = playerTransform.position + Vector3.up * yOffset;
            if (playerInput.ThirdPersonCamera.Aim.IsPressed())
            {
                // if the player is holding down the Aim button, set the aim lock
                isAimLocked = true;
            }
            else if (!playerInput.ThirdPersonCamera.Aim.IsPressed())
            {
                // if the player is not holding down the Aim button, reset the aim lock
                isAimLocked = false;

                // if the aim is not locked, allow the player to rotate the camera
                float rotateInput = playerInput.ThirdPersonCamera.Rotate.ReadValue<float>();

                // multiplying by 360 so a rotation speed of 1 represents a full rotation in 1 second
                transform.Rotate(new Vector3(0f, rotateInput * rotateSpeed * 360f * Time.deltaTime, 0f));
            }

            if (isAimLocked)
            {
                // if we're aim locked keep the camera pointed towards the player forward
                transform.forward = Vector3.Lerp(
                    transform.forward,
                    playerTransform.forward,
                    aimSpeed * Time.deltaTime
                );
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

        public void ResetCameraPosition(Vector3 position)
        {
            transform.position = position;
            cameraController.ResetCameraPosition(position);
        }

        private void ZoomCamera(InputAction.CallbackContext ctx)
        {
            cameraController.CycleFollowOffset();
        }

        private void FixedRotate(InputAction.CallbackContext ctx)
        {
            if (isAimLocked)
            {
                return;
            }

            float value = ctx.ReadValue<float>();
            float fixedRotation = CalculateCurrentRotationIncrement() + value * fixedRotateIncrement;

            transform.rotation = Quaternion.Euler(new Vector3(0f, fixedRotation, 0f));
        }

        private float CalculateCurrentRotationIncrement()
        {
            int currentRotation = (int)transform.rotation.eulerAngles.y;
            int incrementRotation = currentRotation % fixedRotateIncrement;
            if (incrementRotation < fixedRotateIncrement / 2f)
            {
                return (float)currentRotation - incrementRotation;
            }

            return (float)currentRotation - incrementRotation + fixedRotateIncrement;
        }
    }
}
