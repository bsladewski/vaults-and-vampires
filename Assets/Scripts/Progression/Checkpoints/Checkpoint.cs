using Sirenix.OdinInspector;
using UnityEngine;

namespace Progression
{
    public class Checkpoint : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The transform used for repositioning the player after a respawn.")]
        [Required]
        [SerializeField]
        private Transform checkpointPositionTransform;

        [FoldoutGroup("Settings")]
        [Tooltip("Major checkpoints are returned after the player dies. Minor checkpoints are returned to if the player falls off the map.")]
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
