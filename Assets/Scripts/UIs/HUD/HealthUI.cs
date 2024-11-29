using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Player
{
    public class HealthUI : MonoBehaviour
    {
        [FoldoutGroup("Dependencies")]
        [Tooltip("Tracks when the player gains or loses health.")]
        [Required]
        [SerializeField]
        private HealthManager healthManager;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The transform that health icons should be a child of.")]
        [Required]
        [SerializeField]
        private Transform healthIconUIParent;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The prefab used for instantiating health icons.")]
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
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
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
