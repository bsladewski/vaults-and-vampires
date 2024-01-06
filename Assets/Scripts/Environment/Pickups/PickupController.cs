using UnityEngine;

public class PickupController : MonoBehaviour
{
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
            }
            else
            {
                pickup.GetPickup();
            }
        }
    }
}
