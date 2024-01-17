using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Player
{
    public class HealthUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [Required]
        [SerializeField]
        private Transform healthIconUIParent;

        [Required]
        [SerializeField]
        private HealthIconUI healthIconUIPrefab;

        private List<HealthIconUI> healthIcons;

        private void Start()
        {
            healthIcons = new List<HealthIconUI>();
            for (int i = 0; i < healthManager.GetMaxHealth(); i++)
            {
                HealthIconUI healthIcon = Instantiate(healthIconUIPrefab, healthIconUIParent);
                healthIcons.Add(healthIcon);
            }
            UpdateHealthIcons();
        }

        private void OnEnable()
        {
            healthManager.OnHealthGained += OnHealthGained;
            healthManager.OnHealthLost += OnHealthLost;
        }

        private void OnDisable()
        {
            healthManager.OnHealthGained -= OnHealthGained;
            healthManager.OnHealthLost -= OnHealthLost;
        }

        private void OnHealthGained()
        {
            UpdateHealthIcons();
        }

        private void OnHealthLost()
        {
            foreach (HealthIconUI healthIcon in healthIcons)
            {
                healthIcon.PlayHealthLostFeedbacks();
            }
            UpdateHealthIcons();
        }

        private void UpdateHealthIcons()
        {
            int currentHealth = healthManager.GetCurrentHealth();
            for (int i = 0; i < healthIcons.Count; i++)
            {
                if (i + 1 <= currentHealth)
                {
                    healthIcons[i].ShowFilled();
                }
                else
                {
                    healthIcons[i].ShowEmpty();
                }
            }
        }
    }
}
