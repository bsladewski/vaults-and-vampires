using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    [SelectionBase]
    public class SpellUnlock : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Spawned when a player picks up the unlock.")]
        [Required]
        [SerializeField]
        private ParticleSystem unlockParticleSystemPrefab;

        [FoldoutGroup("Settings")]
        [Tooltip("The type of spell unlocked when picked up.")]
        [SerializeField]
        private SpellType spellType;

        [FoldoutGroup("Settings")]
        [Tooltip("The spawn point for the pick up particle effect.")]
        [SerializeField]
        private Transform particleSystemSpawnPoint;

        public SpellType GetSpellType()
        {
            return spellType;
        }

        public void PickupSpellUnlock()
        {
            Instantiate(unlockParticleSystemPrefab, particleSystemSpawnPoint.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
