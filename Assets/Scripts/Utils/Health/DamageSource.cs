using UnityEngine;

namespace Utils
{
    [SelectionBase]
    public class DamageSource : MonoBehaviour
    {
        [SerializeField]
        private int damageAmount = 1;

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
