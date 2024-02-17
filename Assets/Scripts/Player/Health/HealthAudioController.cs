using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Player
{
    public class HealthAudioController : RandomClipPlayer
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles taking damage from damage sources.")]
        [Required]
        [SerializeField]
        private DamageReceiver damageReceiver;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player takes blunt damage.")]
        [Required]
        [SerializeField]
        private AudioClip[] bluntDamageClips;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player takes piercing damage.")]
        [Required]
        [SerializeField]
        private AudioClip[] piercingDamageClips;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player takes slashing damage.")]
        [Required]
        [SerializeField]
        private AudioClip[] slashingDamageClips;

        [FoldoutGroup("Audio Clips")]
        [Tooltip("Played when the player hits the ground after dying.")]
        [Required]
        [SerializeField]
        private AudioClip[] deathHitGroundClips;

        private void OnEnable()
        {
            damageReceiver.OnDamageTaken += OnDamageTaken;
        }

        private void OnDisable()
        {
            damageReceiver.OnDamageTaken -= OnDamageTaken;
        }

        public void PlayHitGroundAfterDeathClip()
        {
            PlayRandomClip(deathHitGroundClips);
        }

        private void OnDamageTaken(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Blunt:
                    PlayRandomClip(bluntDamageClips);
                    break;
                case DamageType.Piercing:
                    PlayRandomClip(piercingDamageClips);
                    break;
                case DamageType.Slashing:
                    PlayRandomClip(slashingDamageClips);
                    break;
                default:
                    Debug.LogErrorFormat("Encountered unimplemented damage type: {0}", damageType);
                    break;
            }
        }
    }
}
