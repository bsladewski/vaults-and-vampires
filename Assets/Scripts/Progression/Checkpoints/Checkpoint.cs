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

        [Header("Settings")]
        [SerializeField]
        private bool isMajorCheckpoint;

        public Vector3 GetCheckpointPosition()
        {
            return checkpointPositionTransform.position;
        }

        public bool GetIsMajorCheckpoint()
        {
            return isMajorCheckpoint;
        }
    }
}
