using System;
using UnityEngine;

namespace Utils
{
    public class HealthManager : MonoBehaviour
    {
        public Action OnHealthGained;

        public Action OnHealthLost;

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

            if (amount > 0)
            {
                OnHealthGained?.Invoke();
            }
            else if (amount < 0)
            {
                OnHealthLost?.Invoke();
            }

            if (currentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            OnHealthGained?.Invoke();
        }
    }
}
