using Sirenix.OdinInspector;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace Player
{
    public class MovementAnimationController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Controls player animations states during movement.")]
        [Required]
        [SerializeField]
        private Animator animator;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Handles movement of player kinematic rigidbody.")]
        [Required]
        [SerializeField]
        private MovementController movementController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Handles input related to movement.")]
        [Required]
        [SerializeField]
        private MovementInputController movementInputController;

        [FoldoutGroup("Feedbacks")]
        [Tooltip("Feedbacks played when the player jumps.")]
        [Required]
        [SerializeField]
        private MMF_Player jumpFeedbacks;

        [FoldoutGroup("Feedbacks")]
        [Tooltip("Feedbacks played when the player double jumps.")]
        [Required]
        [SerializeField]
        private MMF_Player doubleJumpFeedbacks;

        [FoldoutGroup("Feedbacks")]
        [Tooltip("Feedbacks played when the player lands.")]
        [Required]
        [SerializeField]
        private MMF_Player softLandingFeedbacks;

        [FoldoutGroup("Feedbacks")]
        [Tooltip("Feedbacks played when the player takes damage on landing.")]
        [Required]
        [SerializeField]
        private MMF_Player hardLandingFeedbacks;

        [FoldoutGroup("Settings")]
        [Tooltip("Controls how quickly animations should respond to changes in player movement.")]
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
                if (!wasAimLocked)
                {
                    horizontalAnimationSpeed = 0f;
                }

                wasAimLocked = isAimLocked;
                animator.SetFloat("Aim Lock", isAimLocked ? 1f : 0f);
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

            animator.SetFloat("Forward Speed", moveAnimationSpeed);
            animator.SetFloat("Total Speed", moveAnimationSpeed);
            animator.SetFloat("Rotation", 0f);
        }

        private void HandleAimLockedMovementAnimations()
        {
            Vector3 movementDirection = movementInputController.GetMovementDirection();
            moveAnimationSpeed = Mathf.Lerp(
                moveAnimationSpeed,
                movementDirection.z,
                Time.deltaTime * moveAnimationAcceleration
            );

            horizontalAnimationSpeed = Mathf.Lerp(
                horizontalAnimationSpeed,
                movementDirection.x,
                Time.deltaTime * moveAnimationAcceleration
            );

            animator.SetFloat("Forward Speed", moveAnimationSpeed);
            animator.SetFloat("Horizontal Speed", horizontalAnimationSpeed);
            animator.SetFloat("Total Speed", moveAnimationSpeed + horizontalAnimationSpeed);
            animator.SetFloat("Rotation", movementInputController.GetAimLockRotation());
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
