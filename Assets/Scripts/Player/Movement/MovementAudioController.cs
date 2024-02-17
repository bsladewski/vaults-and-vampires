using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Player
{
    public class MovementAudioController : RandomClipPlayer
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles movement of player kinematic rigidbody.")]
        [Required]
        [SerializeField]
        private MovementController movementController;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played to represent footsteps.")]
        [Required]
        [SerializeField]
        private AudioClip[] footstepClips;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player lands.")]
        [Required]
        [SerializeField]
        private AudioClip[] landClips;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player takes fall damage from landing.")]
        [Required]
        [SerializeField]
        private AudioClip[] hardLandingClips;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player double jumps.")]
        [Required]
        [SerializeField]
        private AudioClip[] doubleJumpClips;

        private void OnEnable()
        {
            movementController.OnLanded += OnLanded;
            movementController.OnHardLanding += OnHardLanding;
            movementController.OnDoubleJumped += OnDoubleJumped;
        }

        private void OnDisable()
        {
            movementController.OnLanded -= OnLanded;
            movementController.OnHardLanding -= OnHardLanding;
            movementController.OnDoubleJumped -= OnDoubleJumped;
        }

        public void PlayFootstepClip()
        {
            PlayRandomClip(footstepClips);
        }

        private void OnLanded()
        {
            PlayRandomClip(landClips);
        }

        private void OnHardLanding()
        {
            PlayRandomClip(hardLandingClips);
        }

        private void OnDoubleJumped()
        {
            PlayRandomClip(doubleJumpClips);
        }
    }
}
