using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Pickup : MonoBehaviour
{
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
