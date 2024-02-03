using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class Bob : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("How quickly the bob effect should oscillate in seconds.")]
        [SerializeField]
        private float bobSpeed = 2f;

        [FoldoutGroup("Settings")]
        [SerializeField]
        private float bobHeight = 0.1f;

        [FoldoutGroup("Settings")]
        [Tooltip("The global time offset for the bob effect.")]
        [SerializeField]
        private float phaseOffset;

        private void Update()
        {
            float height = Mathf.Sin(Time.time * bobSpeed + phaseOffset) * bobHeight;
            transform.localPosition = new Vector3(transform.localPosition.x, height, transform.localPosition.z);
        }
    }
}
