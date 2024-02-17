using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    public class CollectibleAudioController : RandomClipPlayer
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles picking up collectibles.")]
        [Required]
        [SerializeField]
        private CollectibleInteractionController collectibleInteractionController;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when a collectible is picked up.")]
        [Required]
        [SerializeField]
        private AudioClip[] collectiblePickedUpClips;

        private void OnEnable()
        {
            collectibleInteractionController.OnCollectiblePickedUp += OnCollectiblePickedUp;
        }

        private void OnDisable()
        {
            collectibleInteractionController.OnCollectiblePickedUp -= OnCollectiblePickedUp;
        }

        private void OnCollectiblePickedUp()
        {
            PlayRandomClip(collectiblePickedUpClips);
        }
    }
}
