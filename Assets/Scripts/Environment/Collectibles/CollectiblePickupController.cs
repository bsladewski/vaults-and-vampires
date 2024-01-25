using UnityEngine;
using Utils;

namespace Environment
{
    public class CollectiblePickupController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private LayerMask collectibleLayerMask;

        private void OnTriggerEnter(Collider collider)
        {
            if (CollisionUtils.IsColliderInLayerMask(collider, collectibleLayerMask))
            {
                Collectible collectible = collider.GetComponent<Collectible>();
                if (collectible == null)
                {
                    Debug.LogError("Collectible is missing Collectible component!");
                    return;
                }

                collectible.Pickup();
            }
        }
    }
}
