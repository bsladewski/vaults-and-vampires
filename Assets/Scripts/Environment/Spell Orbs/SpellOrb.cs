using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Environment
{
    [SelectionBase]
    public class SpellOrb : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private GameObject spellOrbVisual;

        [Header("Settings")]
        [SerializeField]
        private SpellType spellType;

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
