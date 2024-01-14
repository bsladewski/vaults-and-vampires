using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    [SelectionBase]
    public class Pickup : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private PickupVisual pickupVisual;

        public void GetPickup()
        {
            if (pickupVisual.GetPickupParticleSystemPrefab() != null)
            {
                Instantiate(
                    pickupVisual.GetPickupParticleSystemPrefab(),
                    pickupVisual.transform.position,
                    Quaternion.identity
                );
            }

            Destroy(gameObject);
        }
    }
}
