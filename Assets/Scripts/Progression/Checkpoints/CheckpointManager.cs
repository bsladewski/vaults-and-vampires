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

        private List<Vector3> visitedCheckpoints;

        private void Start()
        {
            visitedCheckpoints = new List<Vector3>();
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

        public void ClearMinorCheckpoints()
        {
            for (int i = visitedCheckpoints.Count - 1; i >= 0; i--)
            {
                if (!visitedCheckpoints[i].Equals(majorRespawnPoint))
                {
                    visitedCheckpoints.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            respawnPoint = majorRespawnPoint;
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
