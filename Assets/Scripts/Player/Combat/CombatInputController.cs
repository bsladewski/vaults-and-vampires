using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class CombatInputController : MonoBehaviour
    {
        [FoldoutGroup("Settings", expanded: true)]
        [Tooltip("The time without combat actions before the player exits the combat state.")]
        [SerializeField]
        private float combatTimeout = 6f;

        private PlayerInput playerInput;

        private float combatTimeoutTimer;

        private bool isInCombat;

        private void Awake()
        {
            playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            playerInput.Enable();
            playerInput.ThirdPersonCombat.Attack.performed += OnAttack;
        }

        private void OnDisable()
        {
            playerInput.ThirdPersonCombat.Attack.performed -= OnAttack;
        }

        private void Update()
        {
            if (combatTimeoutTimer > 0f)
            {
                combatTimeoutTimer -= Time.deltaTime;
                if (combatTimeoutTimer <= 0f)
                {
                    isInCombat = false;
                }
            }
        }

        public bool GetIsInCombat()
        {
            return isInCombat;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            isInCombat = true;
            combatTimeoutTimer = combatTimeout;
        }
    }
}
