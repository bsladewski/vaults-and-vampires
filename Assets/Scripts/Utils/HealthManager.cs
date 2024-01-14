using System;
using UnityEngine;

namespace Utils
{
    public class HealthManager : MonoBehaviour
    {
        public Action OnDeath;

        [SerializeField]
        private int maxHealth;

        private int currentHealth;

        private void Awake()
        {
            ResetHealth();
        }

        public void UpdateHealth(int amount)
        {
            currentHealth = Math.Clamp(currentHealth + amount, 0, maxHealth);
            if (currentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }
    }
}
