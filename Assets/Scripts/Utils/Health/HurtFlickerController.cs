using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class HurtFlickerController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Handles receiving damage from damage sources.")]
        [Required]
        [SerializeField]
        private DamageReceiver damageReceiver;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The mesh renderer whose material will be used for the flicker.")]
        [Required]
        [SerializeField]
        private SkinnedMeshRenderer skinnedMeshRenderer;

        [FoldoutGroup("Dependencies")]
        [Tooltip("The material for the flicker effect.")]
        [Required]
        [SerializeField]
        private Material hurtFlickerMaterial;

        [FoldoutGroup("Settings")]
        [Tooltip("The amount of time in seconds it takes to complete one on/off cycle of the flicker.")]
        [MinValue(0f)]
        [SerializeField]
        private float hurtFlickerFrequency = 0.3f;

        private float hurtFlickerTimer;

        private bool hurtFlickerMaterialOn;

        private Material defaultMaterial;

        private bool wasInvulnerable;

        private void Awake()
        {
            defaultMaterial = skinnedMeshRenderer.material;
        }

        private void Update()
        {
            bool isInvulnerable = damageReceiver.IsInvulnerable();
            if (!isInvulnerable && wasInvulnerable)
            {
                // if we stopped being invulnerable reset the material
                skinnedMeshRenderer.material = defaultMaterial;
                hurtFlickerMaterialOn = false;
                hurtFlickerTimer = 0f;
            }

            if (isInvulnerable)
            {
                float hurtFlickerPeriod = hurtFlickerTimer % hurtFlickerFrequency;
                if (hurtFlickerPeriod < hurtFlickerFrequency / 2f && !hurtFlickerMaterialOn)
                {
                    skinnedMeshRenderer.material = hurtFlickerMaterial;
                    hurtFlickerMaterialOn = true;
                }
                else if (hurtFlickerPeriod >= hurtFlickerFrequency / 2f && hurtFlickerMaterialOn)
                {
                    skinnedMeshRenderer.material = defaultMaterial;
                    hurtFlickerMaterialOn = false;
                }

                hurtFlickerTimer += Time.deltaTime;
            }

            wasInvulnerable = isInvulnerable;
        }
    }
}
