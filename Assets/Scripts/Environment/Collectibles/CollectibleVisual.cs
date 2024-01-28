using Sirenix.OdinInspector;
using UnityEngine;

// TODO: This script can be replaced with a scriptable object and the Spin utility script
namespace Environment
{
    public class CollectibleVisual : MonoBehaviour
    {
        [Header("Dependencies")]
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
}
