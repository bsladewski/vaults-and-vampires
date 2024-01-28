using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class HurtFlickerController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private DamageReceiver damageReceiver;

        [Required]
        [SerializeField]
        private SkinnedMeshRenderer skinnedMeshRenderer;

        [Required]
        [SerializeField]
        private Material hurtFlickerMaterial;

        [Header("Settings")]
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
