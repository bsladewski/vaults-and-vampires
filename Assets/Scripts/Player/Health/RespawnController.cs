using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [Header("Dependencies")]
    [Required]
    [SerializeField]
    private MovementController movementController;

    [Required]
    [SerializeField]
    private CheckpointManager checkpointManager;

    [Header("Settings")]
    [SerializeField]
    private float minMapHeight = -10f;

    private void Update()
    {
        if (transform.position.y < minMapHeight)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForEndOfFrame();
        movementController.SetPosition(checkpointManager.GetRespawnPoint());
        ThirdPersonCameraTarget.Instance.ResetCameraPosition(checkpointManager.GetRespawnPoint());
    }
}
