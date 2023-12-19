using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup")]
public class PickupSO : ScriptableObject
{
    [Required]
    [SerializeField]
    private PickupVisual pickupVisualPrefab;

    [SerializeField]
    private ParticleSystem getPickupParticleSystemPrefab;

    public PickupVisual GetPickupVisualPrefab()
    {
        return pickupVisualPrefab;
    }

    public ParticleSystem GetPickupParticleSystemPrefab()
    {
        return getPickupParticleSystemPrefab;
    }
}
