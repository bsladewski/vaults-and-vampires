using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    [SelectionBase]
    public class SpellOrb : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("A reference to the visual that is displayed when a spell orb is active.")]
        [Required]
        [SerializeField]
        private GameObject spellOrbVisual;

        [FoldoutGroup("Settings")]
        [Tooltip("The type of spell triggered when this spell orb is consumed.")]
        [SerializeField]
        private SpellType spellType;

        [FoldoutGroup("Settings")]
        [Tooltip("How long it takes for the spell orb to respawn after consumption.")]
        [SerializeField]
        private float respawnDuration = 5f;

        private bool isActive = true;

        public SpellType GetSpellType()
        {
            return spellType;
        }

        public bool GetIsActive()
        {
            return isActive;
        }

        public void ConsumeSpellOrb()
        {
            if (!isActive)
            {
                return;
            }

            spellOrbVisual.SetActive(false);
            isActive = false;
            StartCoroutine(RespawnSpellOrb());
        }

        private IEnumerator RespawnSpellOrb()
        {
            yield return new WaitForSeconds(respawnDuration);
            spellOrbVisual.SetActive(true);
            isActive = true;
        }
    }
}
