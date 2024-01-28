using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Environment
{
    public class SpellUnlockInteractionController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private UnityEvent<SpellType> unlockEvent;

        [Header("Settings")]
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

                unlockEvent.Invoke(spellUnlock.GetSpellType());
                spellUnlock.PickupSpellUnlock();
            }
        }
    }
}
