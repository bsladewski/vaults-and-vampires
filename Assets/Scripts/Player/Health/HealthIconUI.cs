using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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

        public void ShowFilled()
        {
            image.sprite = filledSprite;
        }

        public void ShowEmpty()
        {
            image.sprite = emptySprite;
        }
    }
}
