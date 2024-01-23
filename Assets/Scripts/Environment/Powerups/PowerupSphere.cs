using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment
{
    [SelectionBase]
    public class PowerupSphere : MonoBehaviour
    {
        public enum PowerupSphereType
        {
            DoubleJump
        }

        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private GameObject powerupSphereVisual;

        [SerializeField]
        private PowerupSphereType powerupSphereType;

        [Header("Settings")]
        [SerializeField]
        private float respawnDuration = 5f;

        private bool isActive = true;

        public PowerupSphereType GetPowerupSphereType()
        {
            return powerupSphereType;
        }

        public bool GetIsActive()
        {
            return isActive;
        }

        public void ConsumePowerupSphere()
        {
            if (!isActive)
            {
                return;
            }

            powerupSphereVisual.SetActive(false);
            isActive = false;
            StartCoroutine(RespawnPowerupSphere());
        }

        private IEnumerator RespawnPowerupSphere()
        {
            yield return new WaitForSeconds(respawnDuration);
            powerupSphereVisual.SetActive(true);
            isActive = true;
        }
    }
}
