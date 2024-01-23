using UnityEngine;
using Utils;

namespace Environment
{
    public class PickupController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private LayerMask pickupLayerMask;

        private void OnTriggerEnter(Collider collider)
        {
            if (CollisionUtils.IsColliderInLayerMask(collider, pickupLayerMask))
            {
                Pickup pickup = collider.GetComponent<Pickup>();
                if (pickup == null)
                {
                    Debug.LogError("Pickup is missing Pickup component!");
                    return;
                }

                pickup.GetPickup();
            }
        }
    }
}
