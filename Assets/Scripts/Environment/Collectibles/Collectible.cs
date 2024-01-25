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
        private CollectibleVisual collectibleVisual;

        public void Pickup()
        {
            if (collectibleVisual.GetPickupParticleSystemPrefab() != null)
            {
                Instantiate(
                    collectibleVisual.GetPickupParticleSystemPrefab(),
                    collectibleVisual.transform.position,
                    Quaternion.identity
                );
            }

            Destroy(gameObject);
        }
    }
}
