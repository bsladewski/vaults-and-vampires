using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    [SelectionBase]
    public class SpellUnlock : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Spawned when a player unlocks the spell.")]
        [Required]
        [SerializeField]
        private ParticleSystem unlockParticleSystemPrefab;

        [FoldoutGroup("Settings")]
        [Tooltip("The type of spell unlocked.")]
        [SerializeField]
        private SpellType spellType;

        [FoldoutGroup("Settings")]
        [Tooltip("The spawn point for the unlock particle effect.")]
        [SerializeField]
        private Transform particleSystemSpawnPoint;

        public SpellType GetSpellType()
        {
            return spellType;
        }

        public void UnlockSpell()
        {
            Instantiate(unlockParticleSystemPrefab, particleSystemSpawnPoint.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
