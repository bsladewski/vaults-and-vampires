using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup")]
public class PickupSO : ScriptableObject
{
    [Required]
    public PickupVisual pickupVisualPrefab;

    public ParticleSystem getPickupParticleSystemPrefab;
}
