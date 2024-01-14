using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Progression
{
    public class CheckpointManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private LayerMask checkpointLayerMask;

        private Vector3 respawnPoint;

        private Vector3 majorRespawnPoint;

        private HashSet<Vector3> visitedCheckpoints;

        private void Start()
        {
            visitedCheckpoints = new HashSet<Vector3>();
            SetRespawnPoint(transform.position, true);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (CollisionUtils.IsColliderInLayerMask(collider, checkpointLayerMask))
            {
                Checkpoint checkpoint = collider.GetComponent<Checkpoint>();
                if (checkpoint == null)
                {
                    Debug.LogError("Checkpoint is missing Checkpoint component!");
                }
                else
                {
                    Vector3 checkpointPosition = checkpoint.GetCheckpointPosition();
                    if (!visitedCheckpoints.Contains(checkpointPosition))
                    {
                        SetRespawnPoint(checkpointPosition, checkpoint.GetIsMajorCheckpoint());
                    }
                }
            }
        }

        public Vector3 GetRespawnPoint()
        {
            return respawnPoint;
        }

        public Vector3 GetMajorRespawnPoint()
        {
            return majorRespawnPoint;
        }

        private void SetRespawnPoint(Vector3 respawnPoint, bool isMajorCheckpoint)
        {
            visitedCheckpoints.Add(respawnPoint);
            this.respawnPoint = respawnPoint;
            if (isMajorCheckpoint)
            {
                majorRespawnPoint = respawnPoint;
            }
        }
    }
}
