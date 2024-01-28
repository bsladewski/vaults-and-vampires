using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Environment
{
    public class SpellOrbInteractionController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private UnityEvent doubleJumpEvent;

        [Header("Settings")]
        [SerializeField]
        private LayerMask spellOrbLayerMask;

        private void OnTriggerEnter(Collider collider)
        {
            if (CollisionUtils.IsColliderInLayerMask(collider, spellOrbLayerMask))
            {
                SpellOrb spellOrb = collider.gameObject.GetComponent<SpellOrb>();
                if (spellOrb == null)
                {
                    Debug.LogError("Spell orb is missing SpellOrb component!");
                    return;
                }

                if (!spellOrb.GetIsActive())
                {
                    return;
                }

                SpellType spellType = spellOrb.GetSpellType();
                switch (spellType)
                {
                    case SpellType.DoubleJump:
                        spellOrb.ConsumeSpellOrb();
                        doubleJumpEvent.Invoke();
                        break;
                    default:
                        Debug.LogErrorFormat("Unimplemented spell orb type {0}!", spellType);
                        break;
                }
            }
        }
    }
}