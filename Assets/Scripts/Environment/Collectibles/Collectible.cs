using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    [SelectionBase]
    public class Collectible : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Spawned when a collectible is picked up.")]
        [Required]
        [SerializeField]
        private ParticleSystem pickUpParticleSystemPrefab;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The spawn point for the pick up particle effect.")]
        [Required]
        [SerializeField]
        private Transform particleSystemSpawnPoint;

        public void PickUp()
        {
            Instantiate(
                pickUpParticleSystemPrefab,
                particleSystemSpawnPoint.position,
                Quaternion.identity
            );

            Destroy(gameObject);
        }
    }
}
