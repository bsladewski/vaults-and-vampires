using Sirenix.OdinInspector;
using UnityEngine;
using Events;
using Utils;

namespace Player
{
    public class AbilityController : MonoBehaviour
    {
        [FoldoutGroup("Abilities", expanded: true)]
        [Tooltip("Tracks whether the player can double jump.")]
        [SerializeField]
        private bool canDoubleJump;

        private void OnEnable()
        {
            EventsSystem.Instance.abilityEvents.OnSpellUnlocked += OnSpellUnlocked;
        }

        private void OnDisable()
        {
            EventsSystem.Instance.abilityEvents.OnSpellUnlocked -= OnSpellUnlocked;
        }

        private void OnSpellUnlocked(GameObject target, SpellType spellType)
        {
            if (target != gameObject)
            {
                return;
            }

            switch (spellType) {
            case SpellType.DoubleJump:
                canDoubleJump = true;
                break;
            default:
                Debug.LogErrorFormat("Encountered unimplemented spell type: {0}", spellType);
                break;
            }
        }

        public bool GetCanDoubleJump()
        {
            return canDoubleJump;
        }
    }
}
