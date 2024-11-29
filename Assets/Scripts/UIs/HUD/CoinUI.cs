using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.Collections;

namespace UIs
{
    public class CoinUI : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("The spacing between characters when displaying coin amounts (em).")]
        [SerializeField]
        private float characterSpacing = 0.5f;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The text that displays how many gold coins the player has.")]
        [Required]
        [SerializeField]
        private TextMeshProUGUI goldAmountText;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The text that displays how many silver coins the player has.")]
        [Required]
        [SerializeField]
        private TextMeshProUGUI silverAmountText;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The text that displays how many copper coins the player has.")]
        [Required]
        [SerializeField]
        private TextMeshProUGUI copperAmountText;

        private int totalAmount;

        private void OnEnable()
        {
            StartCoroutine(BindEvents());
        }

        private IEnumerator BindEvents()
        {
            yield return new WaitForEndOfFrame();
            EventsSystem.Instance.collectibleEvents.OnCoinsCollected += OnCoinsCollected;
        }

        private void OnDisable()
        {
            EventsSystem.Instance.collectibleEvents.OnCoinsCollected -= OnCoinsCollected;
        }

        private void OnCoinsCollected(int amount)
        {
            totalAmount += amount;
            UpdateAmountText();
        }

        private void UpdateAmountText()
        {
            int goldAmount = totalAmount / 100;
            int silverAmount = totalAmount / 10 % 10;
            int copperAmount = totalAmount % 10;

            string mSpaceOpenTag = string.Format("<mspace={0}em>", characterSpacing);
            string mSpaceCloseTag = "</mspace>";

            goldAmountText.text = string.Format("{0}{1}{2}", mSpaceOpenTag, goldAmount, mSpaceCloseTag);
            silverAmountText.text = string.Format("{0}{1}{2}", mSpaceOpenTag, silverAmount, mSpaceCloseTag);
            copperAmountText.text = string.Format("{0}{1}{2}", mSpaceOpenTag, copperAmount, mSpaceCloseTag);
        }
    }
}
