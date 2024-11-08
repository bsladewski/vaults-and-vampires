using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using System.Collections;

namespace Environment
{
    [SelectionBase]
    public class FlipPanel : MonoBehaviour, IMoverController
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Manages the movement of the kinematic rigidbody.")]
        [Required]
        [SerializeField]
        private PhysicsMover mover;

        [FoldoutGroup("Settings")]
        [Tooltip("The axis about which the platform will rotate.")]
        [SerializeField]
        private Vector3 flipAxis = Vector3.forward;

        [FoldoutGroup("Settings")]
        [Tooltip("Determines whether the flip is triggered by player jumping or a timer.")]
        [SerializeField]
        private bool onJump;

        [FoldoutGroup("Settings")]
        [HideIf("onJump")]
        [Tooltip("Specifies an offset in seconds for the flip cycle.")]
        [SerializeField]
        private float offsetTime = 0f;

        [FoldoutGroup("Settings")]
        [HideIf("onJump")]
        [Tooltip("Determines how much time in seconds the platform is idle before flipping.")]
        [SerializeField]
        private float idleTime = 2f;

        [FoldoutGroup("Settings")]
        [HideIf("onJump")]
        [Tooltip("Makes the platform shake before flipping to telegraph the flip to the player.")]
        [SerializeField]
        private float shakeTime = 0.5f;

        [FoldoutGroup("Settings")]
        [HideIf("onJump")]
        [Tooltip("Determines the maximum intensity in units of the pre-flip shake effect.")]
        [SerializeField]
        private float maxShakeIntensity = 0.1f;

        [FoldoutGroup("Settings")]
        [Tooltip("Determines how many degrees the platform rotates each time it flips.")]
        [SerializeField]
        private float flipDegrees = 180f;

        [FoldoutGroup("Settings")]
        [Tooltip("Determines how much time it takes in seconds for the platform to rotate to its next position.")]
        [SerializeField]
        private float flipTime = 0.5f;

        [FoldoutGroup("Props")]
        [Tooltip("A list of props to be activated on start.")]
        [SerializeField]
        private GameObject[] props;

        private Vector3 initialPosition;

        private float timer;

        private bool isFlipping;

        private Quaternion lastRotation;

        private Quaternion targetRotation;

        private void Awake()
        {
            mover.MoverController = this;
            initialPosition = mover.transform.position;
            lastRotation = mover.transform.rotation;
            targetRotation = lastRotation;
            timer = -offsetTime;
            UpdateTargetRotation();
        }

        private void Start()
        {
            if (props != null)
            {
                foreach (GameObject prop in props)
                {
                    prop.SetActive(true);
                }
            }
        }

        private void OnEnable()
        {
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
            Events.EventsSystem.Instance.playerEvents.OnPlayerJumped += OnPlayerJumped;
        }

        private void OnDisable()
        {
            Events.EventsSystem.Instance.playerEvents.OnPlayerJumped -= OnPlayerJumped;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            timer += deltaTime;
            goalPosition = mover.transform.position;
            if (isFlipping && timer < flipTime)
            {
                // rotate towards target
                goalRotation = Quaternion.Lerp(lastRotation, targetRotation, timer / flipTime);
                return;
            }

            if (isFlipping && timer >= flipTime)
            {
                // switch to idle
                goalRotation = targetRotation;
                isFlipping = false;
                timer = 0f;
                UpdateTargetRotation();
                return;
            }

            if (shakeTime > 0f && !onJump && !isFlipping && timer >= idleTime - shakeTime)
            {
                // shake the platform
                goalPosition = initialPosition +
                    (Random.onUnitSphere * maxShakeIntensity * Random.value);
            }

            if (!onJump && !isFlipping && timer >= idleTime)
            {
                // switch to flipping
                goalPosition = initialPosition;
                isFlipping = true;
                timer = 0f;
            }

            // idle
            goalRotation = mover.transform.rotation;
        }

        private void UpdateTargetRotation()
        {
            lastRotation = targetRotation;
            targetRotation = lastRotation * Quaternion.Euler(flipAxis * flipDegrees);
        }

        private void OnPlayerJumped()
        {
            if (!onJump || isFlipping)
            {
                return;
            }

            isFlipping = true;
            timer = 0f;
        }
    }
}
