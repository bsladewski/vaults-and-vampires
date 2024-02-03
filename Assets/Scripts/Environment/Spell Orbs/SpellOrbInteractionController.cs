using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Environment
{
    public class SpellOrbInteractionController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The event fired when a player consumes a double jump spell orb.")]
        [Required]
        [SerializeField]
        private UnityEvent doubleJumpEvent;

        [FoldoutGroup("Settings")]
        [Tooltip("Collision layer used for spell orb triggers.")]
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
