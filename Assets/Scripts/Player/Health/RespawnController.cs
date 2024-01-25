using System.Collections;
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Cameras;
using Progression;
using Utils;

namespace Player
{
    public class RespawnController : MonoBehaviour
    {
        public Action OnRespawn;

        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private MovementController movementController;

        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [Required]
        [SerializeField]
        private CheckpointInteractionController checkpointManager;

        [Header("Settings")]
        [SerializeField]
        private float minMapHeight = -10f;

        [SerializeField]
        private float majorRespawnDelay = 4f;

        private void OnEnable()
        {
            healthManager.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            healthManager.OnDeath -= OnDeath;
        }

        private void Update()
        {
            if (transform.position.y < minMapHeight)
            {
                StartCoroutine(RespawnPlayer());
            }
        }

        private void OnDeath()
        {
            StartCoroutine(RespawnPlayerMajor());
        }

        private IEnumerator RespawnPlayer()
        {
            yield return new WaitForEndOfFrame();
            movementController.SetPosition(checkpointManager.GetRespawnPoint());
            ThirdPersonCameraTarget.Instance.ResetCameraPosition(checkpointManager.GetRespawnPoint());
        }

        private IEnumerator RespawnPlayerMajor()
        {
            yield return new WaitForSeconds(majorRespawnDelay);
            yield return new WaitForEndOfFrame();

            Vector3 respawnPoint = checkpointManager.GetMajorRespawnPoint();
            movementController.SetPosition(respawnPoint);
            ThirdPersonCameraTarget.Instance.ResetCameraPosition(respawnPoint);

            checkpointManager.ClearMinorCheckpoints();
            healthManager.ResetHealth();
            OnRespawn?.Invoke();
        }
    }
}
