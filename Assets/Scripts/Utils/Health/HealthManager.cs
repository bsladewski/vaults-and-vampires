using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class HealthManager : MonoBehaviour
    {
        public Action OnHealthGained;

        public Action OnHealthLost;

        public Action OnDeath;

        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("The amount of health this object will have at full health.")]
        [MinValue(1)]
        [SerializeField]
        private int maxHealth;

        private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
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
