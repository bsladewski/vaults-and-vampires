using Sirenix.OdinInspector;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Required]
    [SerializeField]
    private PickupSO pickupSO;

    [Required]
    [SerializeField]
    private Transform pickupVisualSpawn;

    private void Awake()
    {
        Instantiate(
            pickupSO.GetPickupVisualPrefab(),
            pickupVisualSpawn.position,
            pickupSO.GetPickupVisualPrefab().transform.rotation,
            transform
        );
    }

    public void GetPickup()
    {
        if (pickupSO.GetPickupParticleSystemPrefab() != null)
        {
            Instantiate(
                pickupSO.GetPickupParticleSystemPrefab(),
                pickupVisualSpawn.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
