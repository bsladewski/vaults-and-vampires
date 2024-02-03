using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Environment
{
    public class SpellUnlockInteractionController : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The game object that should receive an interaction event.")]
        [Required]
        [SerializeField]
        private GameObject target;

        [FoldoutGroup("Settings")]
        [Tooltip("Collision layer used for spell orb unlocks.")]
        [SerializeField]
        private LayerMask spellUnlockLayerMask;

        private void OnTriggerEnter(Collider collider)
        {
            if (CollisionUtils.IsColliderInLayerMask(collider, spellUnlockLayerMask))
            {
                SpellUnlock spellUnlock = collider.gameObject.GetComponent<SpellUnlock>();
                if (spellUnlock == null)
                {
                    Debug.LogError("Spell unlock is missing SpellUnlock component!");
                    return;
                }

                EventsSystem.Instance.abilityEvents.OnSpellUnlocked.Invoke(target, spellUnlock.GetSpellType());
                spellUnlock.UnlockSpell();
            }
        }
    }
}
