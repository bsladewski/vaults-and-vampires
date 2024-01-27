using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    [SelectionBase]
    public class SpellUnlock : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private ParticleSystem unlockParticleSystemPrefab;

        [Header("Settings")]
        [SerializeField]
        private SpellType spellType;

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
