using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class DamageReceiver : MonoBehaviour
    {
        public Action<Vector3, float> OnKnockback;

        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [Header("Settings")]
        [SerializeField]
        private float iFrameDuration = 0.8f;

        private float iFrameTimer;

        private void OnEnable()
        {
            healthManager.OnHealthLost += OnHealthLost;
        }

        private void OnDisable()
        {
            healthManager.OnHealthLost -= OnHealthLost;
        }

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
            OnKnockback?.Invoke(
                (transform.position - damageSource.transform.position).normalized,
                damageSource.GetKnockbackIntensity()
            );
        }

        public bool IsInvulnerable()
        {
            return iFrameTimer > 0f;
        }

        private void OnHealthLost()
        {
            iFrameTimer = iFrameDuration;
        }
    }
}
