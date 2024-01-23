using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Environment
{
    public class PowerupSphereController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private UnityEvent doubleJumpEvent;

        [Header("Settings")]
        [SerializeField]
        private LayerMask powerupSphereLayerMask;

        private void OnTriggerEnter(Collider collider)
        {
            if (CollisionUtils.IsColliderInLayerMask(collider, powerupSphereLayerMask))
            {
                PowerupSphere powerupSphere = collider.gameObject.GetComponent<PowerupSphere>();
                if (powerupSphere == null)
                {
                    Debug.LogError("Powerup sphere is missing PowerupSphereComponent!");
                    return;
                }

                if (!powerupSphere.GetIsActive())
                {
                    return;
                }

                switch (powerupSphere.GetPowerupSphereType())
                {
                    case PowerupSphere.PowerupSphereType.DoubleJump:
                        powerupSphere.ConsumePowerupSphere();
                        doubleJumpEvent.Invoke();
                        break;
                    default:
                        Debug.LogError("Encountered invalid PowerupSphereType!");
                        break;
                }
            }
        }
    }
}
