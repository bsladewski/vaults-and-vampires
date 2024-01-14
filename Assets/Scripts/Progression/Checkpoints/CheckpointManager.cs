using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private LayerMask checkpointLayerMask;

    private Vector3 respawnPoint;

    private HashSet<Vector3> visitedCheckpoints;

    private void Start()
    {
        visitedCheckpoints = new HashSet<Vector3>();
        SetRespawnPoint(transform.position);
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
                    SetRespawnPoint(checkpointPosition);
                }
            }
        }
    }

    public Vector3 GetRespawnPoint()
    {
        return respawnPoint;
    }

    private void SetRespawnPoint(Vector3 respawnPoint)
    {
        visitedCheckpoints.Add(respawnPoint);
        this.respawnPoint = respawnPoint;
    }
}
