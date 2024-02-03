using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras
{
    public class ThirdPersonCameraTarget : MonoBehaviour
    {
        public static ThirdPersonCameraTarget Instance { get; private set; }

        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The camera controller for third-person gameplay.")]
        [Required]
        [SerializeField]
        private ThirdPersonCameraController cameraController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The transform whose position the camera target should follow.")]
        [Required]
        [SerializeField]
        private Transform targetTransform;

        [FoldoutGroup("Settings")]
        [Tooltip("How quickly the camera should rotate during free rotation.")]
        [MinValue(0f)]
        [SerializeField]
        private float rotateSpeed = 0.5f;

        [FoldoutGroup("Settings")]
        [Tooltip("How many degrees the camera should rotate during fixed rotation.")]
        [PropertyRange(0f, 180f)]
        [SerializeField]
        private int fixedRotateIncrement = 45;

        [FoldoutGroup("Settings")]
        [Tooltip("How quickly the camera should adjust during aim locked movement.")]
        [MinValue(0f)]
        [SerializeField]
        private float aimSpeed = 15f;

        [FoldoutGroup("Settings")]
        [Tooltip("Adjusts the height of the camera target with respect to the follow target.")]
        [SerializeField]
        private float yOffset = 0.75f;

        private bool isAimLocked;

        private PlayerInput playerInput;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Singleton instance ThirdPersonCameraTarget already instantiated!");
            }
            Instance = this;

            transform.position = targetTransform.position + Vector3.up * yOffset;
            playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            playerInput.Enable();
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
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
            transform.position = targetTransform.position + Vector3.up * yOffset;
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
                    targetTransform.forward,
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

        private void ZoomCamera(InputAction.CallbackContext context)
        {
            cameraController.CycleFollowOffset();
        }

        private void FixedRotate(InputAction.CallbackContext context)
        {
            if (isAimLocked)
            {
                return;
            }

            float value = context.ReadValue<float>();
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
