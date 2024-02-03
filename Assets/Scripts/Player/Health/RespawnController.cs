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

        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles movement of player kinematic rigidbody.")]
        [Required]
        [SerializeField]
        private MovementController movementController;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Tracks health and emits health related events.")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Tracks checkpoints visited by the player.")]
        [Required]
        [SerializeField]
        private CheckpointInteractionController checkpointManager;

        [FoldoutGroup("Settings")]
        [Tooltip("The height a which we consider the player to have fallen off the map.")]
        [SerializeField]
        private float minMapHeight = -10f;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds before the player respawns after dying.")]
        [MinValue(0f)]
        [SerializeField]
        private float majorRespawnDelay = 4f;

        private void OnEnable()
        {
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
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
