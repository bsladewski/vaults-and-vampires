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
        private Animator animator;

        [Required]
        [SerializeField]
        private MovementController movementController;

        [Required]
        [SerializeField]
        private MovementInputController movementInputController;

        [Header("Feedbacks")]
        [Required]
        [SerializeField]
        private MMF_Player jumpFeedbacks;

        [Required]
        [SerializeField]
        private MMF_Player doubleJumpFeedbacks;

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

        private void OnEnable()
        {
            movementController.OnJumped += OnJumped;
            movementController.OnDoubleJumped += OnDoubleJumped;
            movementController.OnFell += OnFell;
            movementController.OnLanded += OnLanded;
        }

        private void OnDisable()
        {
            movementController.OnJumped -= OnJumped;
            movementController.OnDoubleJumped -= OnDoubleJumped;
            movementController.OnFell -= OnFell;
            movementController.OnLanded -= OnLanded;
        }

        private void LateUpdate()
        {
            bool isAimLocked = movementInputController.GetIsAimLocked();
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
            float speedNormalized = movementInputController.GetMovementDirection().magnitude;
            moveAnimationSpeed = Mathf.Lerp(
                moveAnimationSpeed,
                speedNormalized,
                Time.deltaTime * moveAnimationAcceleration);
            animator.SetFloat("Speed", moveAnimationSpeed);
        }

        private void HandleAimLockedMovementAnimations()
        {
            Vector3 movementDirection = movementInputController.GetMovementDirection();
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

        private void OnJumped()
        {
            OnAnyJump();
            jumpFeedbacks.PlayFeedbacks();
        }

        private void OnDoubleJumped()
        {
            OnAnyJump();
            doubleJumpFeedbacks.PlayFeedbacks();
        }

        private void OnAnyJump()
        {
            ResetTriggers();
            animator.SetBool("Mirror Jump", mirrorJump);
            animator.SetTrigger("Jump");
            mirrorJump = !mirrorJump;
        }

        private void OnFell()
        {
            ResetTriggers();
            animator.SetTrigger("Fall");
        }

        private void OnLanded()
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
