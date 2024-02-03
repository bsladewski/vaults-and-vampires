using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Utils
{
    public class FadeMaterialInOut : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The mesh renderer whose material will be faded.")]
        [SerializeField]
        private MeshRenderer meshRenderer;

        [FoldoutGroup("Settings")]
        [Tooltip("The duration in seconds of the fade in effect.")]
        [SerializeField]
        private float fadeInDuration = 0.2f;

        [FoldoutGroup("Settings")]
        [Tooltip("The delay in seconds before the fade out effect begins.")]
        [SerializeField]
        private float fadeOutDelay = 0.2f;

        [FoldoutGroup("Settings")]
        [Tooltip("The duration in seconds of the fade out effect.")]
        [SerializeField]
        private float fadeOutDuration = 0.4f;

        [FoldoutGroup("Settings")]
        [Tooltip("Whether the game object should be destroyed when the fade effect ends.")]
        [SerializeField]
        private bool destroyAfterFadeOut = true;

        private Material material;

        private void Awake()
        {
            material = meshRenderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0f);
        }

        private void Start()
        {
            DOTween.ToAlpha(
                () => material.color,
                value => material.color = value,
                1f,
                fadeInDuration
            ).OnComplete(() =>
            {
                Tween tween = DOTween.ToAlpha(
                    () => material.color,
                    value => material.color = value,
                    0f,
                    fadeOutDuration
                ).SetDelay(fadeOutDelay);
                if (destroyAfterFadeOut)
                {
                    tween.OnComplete(() => Destroy(gameObject));
                }
            });
        }
    }
}
