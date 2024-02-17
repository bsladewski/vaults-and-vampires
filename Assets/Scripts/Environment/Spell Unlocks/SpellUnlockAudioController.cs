using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    public class SpellUnlockAudioController : RandomClipPlayer
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles unlocking spells.")]
        [Required]
        [SerializeField]
        private SpellUnlockInteractionController spellUnlockInteractionController;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when a spell is unlocked.")]
        [Required]
        [SerializeField]
        private AudioClip[] unlockSpellClips;

        private void OnEnable()
        {
            spellUnlockInteractionController.OnSpellUnlocked += OnSpellUnlocked;
        }

        private void OnDisable()
        {
            spellUnlockInteractionController.OnSpellUnlocked -= OnSpellUnlocked;
        }

        private void OnSpellUnlocked()
        {
            PlayRandomClip(unlockSpellClips);
        }
    }
}
