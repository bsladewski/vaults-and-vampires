using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    [SelectionBase]
    public class Collectible : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private ParticleSystem pickupParticleSystemPrefab;

        [Required]
        [SerializeField]
        private Transform particleSystemSpawnPoint;

        public void Pickup()
        {
            Instantiate(
                pickupParticleSystemPrefab,
                particleSystemSpawnPoint.position,
                Quaternion.identity
            );

            Destroy(gameObject);
        }
    }
}
