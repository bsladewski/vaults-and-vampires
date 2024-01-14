using Sirenix.OdinInspector;
using UnityEngine;

namespace Progression
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private Transform checkpointPositionTransform;

        public Vector3 GetCheckpointPosition()
        {
            return checkpointPositionTransform.position;
        }
    }
}
