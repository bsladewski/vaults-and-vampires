using UnityEngine;

namespace Utils
{
    public class Spin : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;

        [SerializeField]
        private float spinSpeed = 60f;

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
