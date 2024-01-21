using UnityEngine;
using DG.Tweening;

namespace Utils
{
    public class FadeMaterialInOut : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private MeshRenderer meshRenderer;

        [Header("Settings")]
        [SerializeField]
        private float fadeInDuration = 0.2f;

        [SerializeField]
        private float fadeOutDelay = 0.2f;

        [SerializeField]
        private float fadeOutDuration = 0.4f;

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
