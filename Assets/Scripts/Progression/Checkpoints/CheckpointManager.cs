using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required]
    [SerializeField]
    private MovementController playerCharacterController;

    [Header("Settings")]
    [SerializeField]
    private LayerMask checkpointLayerMask;

    [SerializeField]
    private float respawnHeight = -10f;

    private Vector3 respawnPoint;

    private HashSet<Vector3> visitedCheckpoints;

    private void Start()
    {
        visitedCheckpoints = new HashSet<Vector3>();
        SetRespawnPoint(transform.position);
    }

    private void LateUpdate()
    {
        if (transform.position.y < respawnHeight)
        {
            StartCoroutine(RespawnCharacterCoroutine());
        }
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

    private void SetRespawnPoint(Vector3 respawnPoint)
    {
        visitedCheckpoints.Add(respawnPoint);
        this.respawnPoint = respawnPoint;
    }

    private IEnumerator RespawnCharacterCoroutine()
    {
        yield return new WaitForEndOfFrame();
        playerCharacterController.SetPosition(respawnPoint);
        ThirdPersonCameraTarget.Instance.ResetCameraPosition(respawnPoint);
    }
}
