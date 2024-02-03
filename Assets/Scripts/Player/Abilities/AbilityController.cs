using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class AbilityController : MonoBehaviour
    {
        [FoldoutGroup("Abilities", expanded: true)]
        [Tooltip("Tracks whether the player can double jump.")]
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
