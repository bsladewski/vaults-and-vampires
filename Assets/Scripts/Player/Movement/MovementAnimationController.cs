using Sirenix.OdinInspector;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace Player
{
    public class MovementAnimationController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private MovementController movementController;

        [Required]
        [SerializeField]
        private MovementInputController thirdPersonMovement;

        [Required]
        [SerializeField]
        private Animator animator;

        [Header("Feedbacks")]
        [Required]
        [SerializeField]
        private MMF_Player jumpFeedbacks;

        [Required]
        [SerializeField]
        private MMF_Player softLandingFeedbacks;

        [Required]
        [SerializeField]
        private MMF_Player hardLandingFeedbacks;

        [Header("Settings")]
        [SerializeField]
        private float moveAnimationAcceleration = 20f;

        private float moveAnimationSpeed;

        private float horizontalAnimationSpeed;

        private bool wasAimLocked;

        private bool mirrorJump;

        private void Start()
        {
            movementController.OnJumped += OnPlayerJumped;
            movementController.OnFell += OnPlayerFell;
            movementController.OnLanded += OnPlayerLanded;
        }

        private void LateUpdate()
        {
            bool isAimLocked = thirdPersonMovement.GetIsAimLocked();
            if (isAimLocked != wasAimLocked)
            {
                wasAimLocked = isAimLocked;
                animator.SetBool("Is Aim Locked", isAimLocked);
            }

            if (isAimLocked)
            {
                HandleAimLockedMovementAnimations();
            }
            else
            {
                HandleMovementAnimations();
            }
        }

        private void HandleMovementAnimations()
        {
            float speedNormalized = thirdPersonMovement.GetMovementDirection().magnitude;
            moveAnimationSpeed = Mathf.Lerp(
                moveAnimationSpeed,
                speedNormalized,
                Time.deltaTime * moveAnimationAcceleration);
            animator.SetFloat("Speed", moveAnimationSpeed);
        }

        private void HandleAimLockedMovementAnimations()
        {
            Vector3 movementDirection = thirdPersonMovement.GetMovementDirection();
            moveAnimationSpeed = Mathf.Lerp(
                moveAnimationSpeed,
                movementDirection.x,
                Time.deltaTime * moveAnimationAcceleration
            );
            horizontalAnimationSpeed = Mathf.Lerp(
                horizontalAnimationSpeed,
                movementDirection.z,
                Time.deltaTime * moveAnimationAcceleration
            );
            animator.SetFloat("Speed", moveAnimationSpeed);
            animator.SetFloat("Horizontal Speed", horizontalAnimationSpeed);
        }

        private void OnPlayerJumped()
        {
            ResetTriggers();
            animator.SetBool("Mirror Jump", mirrorJump);
            animator.SetTrigger("Jump");
            mirrorJump = !mirrorJump;
            jumpFeedbacks.PlayFeedbacks();
        }

        private void OnPlayerFell()
        {
            ResetTriggers();
            animator.SetTrigger("Fall");
        }

        private void OnPlayerLanded()
        {
            ResetTriggers();
            animator.SetTrigger("Land");
            if (movementController.GetWasHardLanding())
            {
                hardLandingFeedbacks.PlayFeedbacks();
            }
            else
            {
                softLandingFeedbacks.PlayFeedbacks();
            }
        }

        private void ResetTriggers()
        {
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("Fall");
            animator.ResetTrigger("Land");
        }
    }
}
