using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    [SelectionBase]
    public class SpellOrb : MonoBehaviour
    {
        public enum SpellOrbType
        {
            DoubleJump
        }

        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private GameObject spellOrbVisual;



        [Header("Settings")]
        [SerializeField]
        private SpellOrbType spellOrbType;

        [SerializeField]
        private float respawnDuration = 5f;

        private bool isActive = true;

        public SpellOrbType GetSpellOrbType()
        {
            return spellOrbType;
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
