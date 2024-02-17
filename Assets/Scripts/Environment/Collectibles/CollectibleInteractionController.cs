using System;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    public class CollectibleInteractionController : MonoBehaviour
    {
        public Action OnCollectiblePickedUp;

        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("Collision layers used for collectible triggers.")]
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

                OnCollectiblePickedUp?.Invoke();
                collectible.PickUp();
            }
        }
    }
}
