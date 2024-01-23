using UnityEngine;

namespace Utils
{
    public class Spin : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;

        [SerializeField]
        private float spinSpeed = 60f;

        public void Update()
        {
            transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime);
        }
    }
}
