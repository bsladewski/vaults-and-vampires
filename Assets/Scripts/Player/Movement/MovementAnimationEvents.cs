using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class MovementAnimationEvents : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The controller for animating the player while moving.")]
        [Required]
        [SerializeField]
        private MovementAnimationController movementAnimationController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The controller for playing movement related audio clips.")]
        [Required]
        [SerializeField]
        private MovementAudioController movementAudioController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The controller for playing health related audio clips.")]
        [Required]
        [SerializeField]
        private HealthAudioController healthAudioController;

        public void OnForwardWalkingFootstep()
        {
            if (
                !movementAnimationController.GetIsRunning() &&
                !movementAnimationController.GetIsMovingBackwards() &&
                !movementAnimationController.GetIsStrafing()
            )
            {
                movementAudioController.PlayFootstepClip();
            }
        }

        public void OnForwardRunningFootstep()
        {
            if (
                movementAnimationController.GetIsRunning() &&
                !movementAnimationController.GetIsMovingBackwards() &&
                !movementAnimationController.GetIsStrafing()
            )
            {
                movementAudioController.PlayFootstepClip();
            }
        }

        public void OnBackwardWalkingFootstep()
        {
            if (
                !movementAnimationController.GetIsRunning() &&
                movementAnimationController.GetIsMovingBackwards() &&
                !movementAnimationController.GetIsStrafing()
            )
            {
                movementAudioController.PlayFootstepClip();
            }
        }

        public void OnBackwardRunningFootstep()
        {
            if (
                movementAnimationController.GetIsRunning() &&
                movementAnimationController.GetIsMovingBackwards() &&
                !movementAnimationController.GetIsStrafing()
            )
            {
                movementAudioController.PlayFootstepClip();
            }
        }

        public void OnStrafeWalkingFootstep()
        {
            if (
                !movementAnimationController.GetIsRunning() &&
                !movementAnimationController.GetIsMovingBackwards() &&
                movementAnimationController.GetIsStrafing()
            )
            {
                movementAudioController.PlayFootstepClip();
            }
        }

        public void OnStrafeRunningFootstep()
        {
            if (
                movementAnimationController.GetIsRunning() &&
                !movementAnimationController.GetIsMovingBackwards() &&
                movementAnimationController.GetIsStrafing()
            )
            {
                movementAudioController.PlayFootstepClip();
            }
        }

        public void OnHitGroundAfterDeath()
        {
            healthAudioController.PlayHitGroundAfterDeathClip();
        }
    }
}
