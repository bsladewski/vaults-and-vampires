using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class DamageReceiver : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [SerializeField]
        private float iFrameDuration = 1f;

        private float iFrameTimer;

        private void Update()
        {
            if (iFrameTimer > 0)
            {
                iFrameTimer -= Time.deltaTime;
            }
        }

        public void TakeDamage(DamageSource damageSource)
        {
            healthManager.UpdateHealth(-damageSource.GetDamageAmount());
            iFrameTimer = iFrameDuration;
        }

        public bool IsInvulnerable()
        {
            return iFrameTimer > 0f;
        }
    }
}
