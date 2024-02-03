using System.Collections;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Player
{
    public class HealthAnimationController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Controls player animations states for health events.")]
        [Required]
        [SerializeField]
        private Animator animator;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Tracks health and emits health related events.")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [FoldoutGroup("Dependencies")]
        [Tooltip("Handles respawning the player after falling off the map or dying.")]
        [Required]
        [SerializeField]
        private RespawnController respawnController;

        [FoldoutGroup("Feedbacks")]
        [Tooltip("Feedbacks played when the player loses health.")]
        [Required]
        [SerializeField]
        private MMF_Player healthLostFeedbacks;

        private void OnEnable()
        {
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
            healthManager.OnHealthLost += OnHealthLost;
            healthManager.OnDeath += OnDeath;
            respawnController.OnRespawn += OnRespawn;
        }

        private void OnDisable()
        {
            healthManager.OnHealthLost -= OnHealthLost;
            healthManager.OnDeath -= OnDeath;
            respawnController.OnRespawn -= OnRespawn;
        }

        private void OnHealthLost()
        {
            ResetTriggers();
            animator.SetTrigger("Hit");
            healthLostFeedbacks.PlayFeedbacks();
        }

        private void OnDeath()
        {
            ResetTriggers();
            animator.SetTrigger("Die");
        }

        private void OnRespawn()
        {
            ResetTriggers();
            animator.SetTrigger("Respawn");
        }

        private void ResetTriggers()
        {
            animator.ResetTrigger("Hit");
            animator.ResetTrigger("Die");
            animator.ResetTrigger("Respawn");
        }
    }
}
