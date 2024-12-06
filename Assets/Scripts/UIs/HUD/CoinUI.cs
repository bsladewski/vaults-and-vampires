using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

namespace UIs
{
    public class CoinUI : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("The spacing between characters when displaying coin amounts (em).")]
        [SerializeField]
        private float characterSpacing = 0.5f;

        [FoldoutGroup("Settings")]
        [Tooltip("The y position of the coin UI when it is hidden.")]
        [SerializeField]
        private float yPositionHidden = 200f;

        [FoldoutGroup("Settings")]
        [Tooltip("The y position of the coin UI when it is shown.")]
        [SerializeField]
        private float yPositionShown = -12f;

        [FoldoutGroup("Settings")]
        [Tooltip("How long it takes in seconds before the Coin UI hides after appearing.")]
        [SerializeField]
        private float hideTimer = 3f;

        [FoldoutGroup("Settings")]
        [Tooltip("How long it takes in seconds for the Coin UI to show.")]
        [SerializeField]
        private float coinUIShowDuration = 0.3f;

        [FoldoutGroup("Settings")]
        [Tooltip("The ease used when animating the Coin UI show.")]
        [SerializeField]
        private Ease coinUIShowEase = Ease.OutFlash;

        [FoldoutGroup("Settings")]
        [Tooltip("How long it takes in seconds for the Coin UI to hide.")]
        [SerializeField]
        private float coinUIHideDuration = 0.3f;

        [FoldoutGroup("Settings")]
        [Tooltip("The ease used when animating the Coin UI hide.")]
        [SerializeField]
        private Ease coinUIHideEase = Ease.InFlash;

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

        private RectTransform rectTransform;

        private bool shown;

        private Coroutine hideIdleCoinUIRoutine;

        private Tween toggleCoinUITween;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ResetCoinUI();
        }

        private void OnEnable()
        {
            ResetCoinUI();
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
            ShowCoinUI();
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

        private void ToggleCoinUI(bool show)
        {
            if (toggleCoinUITween != null && toggleCoinUITween.active)
            {
                toggleCoinUITween.Kill();
            }

            Vector2 to = rectTransform.anchoredPosition;
            to = new Vector2(to.x, show ? yPositionShown : yPositionHidden);

            float duration = show ? coinUIShowDuration : coinUIHideDuration;

            toggleCoinUITween = DOTween.To(
                () => rectTransform.anchoredPosition,
                value => rectTransform.anchoredPosition = value,
                to,
                duration
            ).SetEase(show ? coinUIShowEase : coinUIHideEase);
        }

        private void ResetCoinUI()
        {
            Vector2 to = rectTransform.anchoredPosition;
            to = new Vector2(to.x, yPositionHidden);
            rectTransform.anchoredPosition = to;
        }

        private void ShowCoinUI()
        {
            if (hideIdleCoinUIRoutine != null)
            {
                StopCoroutine(hideIdleCoinUIRoutine);
            }

            ToggleCoinUI(true);
            hideIdleCoinUIRoutine = StartCoroutine(HideIdleCoinUI());
        }

        private IEnumerator HideIdleCoinUI()
        {
            yield return new WaitForSeconds(hideTimer);
            ToggleCoinUI(false);
        }
    }
}
