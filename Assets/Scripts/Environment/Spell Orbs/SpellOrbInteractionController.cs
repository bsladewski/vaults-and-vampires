using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Events;

namespace Environment
{
    public class SpellOrbInteractionController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The game object that should receive an interaction event.")]
        [Required]
        [SerializeField]
        private GameObject target;

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
                EventsSystem.Instance.abilityEvents.OnSpellOrbTriggered?.Invoke(target, spellType);
                spellOrb.ConsumeSpellOrb();
            }
        }
    }
}
