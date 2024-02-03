using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class DamageReceiver : MonoBehaviour
    {
        public Action<Vector3, float> OnKnockback;

        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Tracks health and emits health related events.")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds before the object can take damage after receiving damage.")]
        [SerializeField]
        private float iFrameDuration = 0.8f;

        private float iFrameTimer;

        private void OnEnable()
        {
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
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
