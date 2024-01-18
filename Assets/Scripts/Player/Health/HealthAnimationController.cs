using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Player
{
    public class HealthAnimationController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private Animator animator;

        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [Required]
        [SerializeField]
        private RespawnController respawnController;

        [Header("Feedbacks")]
        [Required]
        [SerializeField]
        private MMF_Player healthLostFeedbacks;

        private void OnEnable()
        {
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
