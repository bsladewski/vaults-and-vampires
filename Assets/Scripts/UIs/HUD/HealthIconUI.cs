using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

namespace Player
{
    public class HealthIconUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private Image image;

        [Required]
        [SerializeField]
        private Sprite filledSprite;

        [Required]
        [SerializeField]
        private Sprite emptySprite;

        [Header("Feedbacks")]
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
