using UnityEngine;

namespace Utils
{
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
