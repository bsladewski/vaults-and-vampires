using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    public class TimedSpikes : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("Specifies an offset in seconds for the extend/retract cycle.")]
        [SerializeField]
        private float offsetTime;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds the spike remain retracted.")]
        [SerializeField]
        private float retractedTime = 2f;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds it takes for the spikes to retract.")]
        [SerializeField]
        private float retractTime = 1f;

        [FoldoutGroup("Settings")]
        [Tooltip("How the spikes should ease their retract position.")]
        [SerializeField]
        private Ease retractEase = Ease.InOutBounce;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds the spike remain extended.")]
        [SerializeField]
        private float extendedTime = 1f;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds it takes for the spikes to extend.")]
        [SerializeField]
        private float extendTime = 0.25f;

        [FoldoutGroup("Settings")]
        [Tooltip("How the spikes should ease their extend position.")]
        [SerializeField]
        private Ease extendEase = Ease.Linear;

        [FoldoutGroup("Settings")]
        [Tooltip("How high the spikes travel when they extend.")]
        [SerializeField]
        private float extendHeight = 1.25f;

        [FoldoutGroup("Settings")]
        [Tooltip("How long in seconds the spikes telegraph that they are about to extend.")]
        [SerializeField]
        private float telegraphTime = 1f;

        [FoldoutGroup("Settings")]
        [Tooltip("How high the spikes travel when they telegraph that they are about to extend.")]
        [SerializeField]
        private float telegraphHeight = 0.5f;

        [FoldoutGroup("Settings")]
        [Tooltip("How the spikes should ease their telegraph position.")]
        [SerializeField]
        private Ease telegraphEaseUp = Ease.OutCirc;

        [FoldoutGroup("Settings")]
        [Tooltip("How the spikes should ease their telegraph position.")]
        [SerializeField]
        private Ease telegraphEaseDown = Ease.InCirc;

        [FoldoutGroup("Settings")]
        [Tooltip("The period in seconds between the telegraph and the spikes extending.")]
        [SerializeField]
        private float telegraphPause = 0.25f;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The damage source for the timed spikes.")]
        [Required]
        [SerializeField]
        private Transform spikesDamageSource;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The visual for the timed spikes.")]
        [Required]
        [SerializeField]
        private Transform spikesVisual;

        private Vector3 damageSourceInitialPosition;

        private Vector3 visualInitialPosition;

        private void Awake()
        {
            damageSourceInitialPosition = spikesDamageSource.position;
            visualInitialPosition = spikesVisual.position;
        }

        private void Start()
        {
            Telegraph(offsetTime);
        }

        private void Telegraph(float offset)
        {
            Tween tween = DOTween.To(
                () => spikesVisual.position,
                value => spikesVisual.position = value,
                visualInitialPosition + spikesVisual.up * telegraphHeight,
                telegraphTime / 2f
            ).SetEase(telegraphEaseUp).SetDelay(offset);
            tween.onComplete = () =>
            {
                Tween tween = DOTween.To(
                    () => spikesVisual.position,
                    value => spikesVisual.position = value,
                    visualInitialPosition,
                    telegraphTime / 2f
                ).SetEase(telegraphEaseDown);
                tween.onComplete = () => Extend();
            };
        }

        private void Extend()
        {
            Tween tween = DOTween.To(
                () => spikesVisual.position,
                value =>
                {
                    spikesVisual.position = value;
                    spikesDamageSource.position = value;
                },
                visualInitialPosition + spikesVisual.up * extendHeight,
                extendTime
            ).SetEase(extendEase).SetDelay(telegraphPause);
            tween.onComplete = () => Retract();
        }

        private void Retract()
        {
            Tween tween = DOTween.To(
                () => spikesVisual.position,
                value =>
                {
                    spikesVisual.position = value;
                    spikesDamageSource.position = value;
                },
                visualInitialPosition,
                retractTime
            ).SetEase(retractEase).SetDelay(extendedTime);
            tween.onComplete = () => Telegraph(retractedTime);
        }
    }
}
