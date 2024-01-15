using UnityEngine;

namespace Utils
{
    public static class CollisionUtils
    {
        public static bool IsColliderInLayerMask(Collider collider, LayerMask layerMask)
        {
            return layerMask == (layerMask | 1 << collider.gameObject.layer);
        }
    }
}
