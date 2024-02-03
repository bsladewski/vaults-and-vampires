using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class Spin : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("The axis on which the rotation will take place.")]
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;

        [FoldoutGroup("Settings")]
        [Tooltip("The degrees per second that the object will rotate.")]
        [SerializeField]
        private float spinSpeed = 60f;

        [FoldoutGroup("Settings")]
        [Tooltip("Whether the rotation takes place in world space or local space.")]
        [SerializeField]
        private bool worldSpace = true;

        public void Update()
        {
            transform.Rotate(
                rotationAxis * spinSpeed * Time.deltaTime,
                worldSpace ? Space.World : Space.Self
            );
        }
    }
}
