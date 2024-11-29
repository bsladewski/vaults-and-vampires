using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

namespace Player
{
    public class HealthIconUI : MonoBehaviour
    {
        [FoldoutGroup("Dependencies")]
        [Tooltip("The image used to display one unit of player health.")]
        [Required]
        [SerializeField]
        private Image image;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The sprite displayed when this unit of player health is filled.")]
        [Required]
        [SerializeField]
        private Sprite filledSprite;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The sprite displayed when this unit of player health is empty.")]
        [Required]
        [SerializeField]
        private Sprite emptySprite;

        [FoldoutGroup("Feedbacks")]
        [Tooltip("Feedbacks applied to this icon when the player loses health.")]
        [Required]
        [SerializeField]
        private MMF_Player healthLostFeedbacks;

        public void ShowFilled()
        {
            image.sprite = filledSprite;
        }

        public void ShowEmpty()
        {
            image.sprite = emptySprite;
        }

        public void PlayHealthLostFeedbacks()
        {
            healthLostFeedbacks.PlayFeedbacks();
        }
    }
}
