using Sirenix.OdinInspector;
using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    [Header("Prefabs")]
    [Required]
    [SerializeField]
    private ParticleSystem pickupParticleSystemPrefab;

    [Header("Settings")]
    [SerializeField]
    private float rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * 360f * Time.deltaTime, Space.World);
    }

    public ParticleSystem GetPickupParticleSystemPrefab()
    {
        return pickupParticleSystemPrefab;
    }
}
