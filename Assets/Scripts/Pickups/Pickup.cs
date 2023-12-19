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
            pickupSO.pickupVisualPrefab,
            pickupVisualSpawn.position,
            pickupSO.pickupVisualPrefab.transform.rotation,
            transform
        );
    }

    public void GetPickup()
    {
        if (pickupSO.getPickupParticleSystemPrefab != null)
        {
            Instantiate(
                pickupSO.getPickupParticleSystemPrefab,
                pickupVisualSpawn.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
