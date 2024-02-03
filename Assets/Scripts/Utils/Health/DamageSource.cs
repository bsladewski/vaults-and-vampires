using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    [SelectionBase]
    public class DamageSource : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("The base amount of damage this source inflicts.")]
        [SerializeField]
        private int damageAmount = 1;

        [FoldoutGroup("Settings")]
        [Tooltip("How intense the knockback should be this from this damage source.")]
        [MinValue(0f)]
        [SerializeField]
        private float knockbackIntensity = 1f;

        public int GetDamageAmount()
        {
            return damageAmount;
        }

        public float GetKnockbackIntensity()
        {
            return knockbackIntensity;
        }
    }
}
