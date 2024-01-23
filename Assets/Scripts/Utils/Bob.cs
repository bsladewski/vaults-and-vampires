using UnityEngine;

namespace Utils
{
    public class Bob : MonoBehaviour
    {
        [SerializeField]
        private float bobSpeed = 2f;

        [SerializeField]
        private float bobHeight = 0.1f;

        [SerializeField]
        private float phaseOffset;

        private void Update()
        {
            float height = Mathf.Sin(Time.time * bobSpeed + phaseOffset) * bobHeight;
            transform.localPosition = new Vector3(transform.localPosition.x, height, transform.localPosition.z);
        }
    }
}
