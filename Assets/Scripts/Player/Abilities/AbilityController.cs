using UnityEngine;

namespace Player
{
    public class AbilityController : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField]
        private bool canDoubleJump;

        public void SetCanDoubleJump(bool canDoubleJump)
        {
            this.canDoubleJump = canDoubleJump;
        }

        public bool GetCanDoubleJump()
        {
            return canDoubleJump;
        }
    }
}
