using UnityEngine;

namespace Utils
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField]
        private int damageAmount = 1;

        public int GetDamageAmount()
        {
            return damageAmount;
        }
    }
}
