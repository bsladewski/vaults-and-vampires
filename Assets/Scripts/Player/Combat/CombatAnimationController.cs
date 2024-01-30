using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class CombatAnimationController : MonoBehaviour
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private Animator animator;

        [Required]
        [SerializeField]
        private CombatInputController combatInputController;

        [Required]
        [SerializeField]
        private MovementInputController movementInputController;

        [Required]
        [SerializeField]
        private GameObject leftHandItem;

        [Required]
        [SerializeField]
        private GameObject rightHandItem;

        [Header("Settings")]
        [SerializeField]
        private float armsCombatLayerWeight = 0.75f;

        [SerializeField]
        private float legsCombatSpeedThreshold = 0.01f;

        [SerializeField]
        private float legsCombatLayerWeight = 0.75f;

        [SerializeField]
        private float layerWeightTweenDuration = 0.25f;

        private int leftArmCombatLayerIndex;

        private int rightArmCombatLayerIndex;

        private int legsCombatLayerIndex;

        private bool wasInCombat;

        private bool wasMoving;

        private void Awake()
        {
            leftArmCombatLayerIndex = animator.GetLayerIndex("Left Arm Combat");
            if (leftArmCombatLayerIndex < 0)
            {
                Debug.LogError("Invalid name for left arm combat animation layer!");
            }

            rightArmCombatLayerIndex = animator.GetLayerIndex("Right Arm Combat");
            if (rightArmCombatLayerIndex < 0)
            {
                Debug.LogError("Invalid name for right arm combat animation layer!");
            }

            legsCombatLayerIndex = animator.GetLayerIndex("Legs Combat");
            if (legsCombatLayerIndex < 0)
            {
                Debug.LogError("Invalid name for legs combat animation layer!");
            }
        }

        private void Update()
        {
            bool isInCombat = combatInputController.GetIsInCombat();
            if (wasInCombat && !isInCombat)
            {
                // if we just left combat, clear weights on arms and legs
                TweenAnimationLayerWeight(leftArmCombatLayerIndex, 0f);
                TweenAnimationLayerWeight(rightArmCombatLayerIndex, 0f);
                TweenAnimationLayerWeight(legsCombatLayerIndex, 0f);

                // hide equipment
                leftHandItem.SetActive(false);
                rightHandItem.SetActive(false);
            }
            else if (!wasInCombat && isInCombat)
            {
                // if we just entered combat, move arms to combat position
                TweenAnimationLayerWeight(leftArmCombatLayerIndex, armsCombatLayerWeight);
                TweenAnimationLayerWeight(rightArmCombatLayerIndex, armsCombatLayerWeight);

                // show equipment
                leftHandItem.SetActive(true);
                rightHandItem.SetActive(true);
            }

            if (isInCombat)
            {
                // update legs animation layer weights
                HandleLegsCombatLayer();
            }

            wasInCombat = isInCombat;
        }

        private void HandleLegsCombatLayer()
        {
            float speedNormalized = movementInputController.GetMovementDirection().magnitude;
            if (speedNormalized >= legsCombatSpeedThreshold && !wasMoving)
            {
                // if the player started moving, clear the combat layer weights
                // this prevents the combat stance from making the leg movement look weird
                TweenAnimationLayerWeight(legsCombatLayerIndex, 0f);
                wasMoving = true;
            }
            else if (speedNormalized < legsCombatSpeedThreshold && (wasMoving || !wasInCombat))
            {
                // if the player stopped moving, set the combat layer weights
                // this makes the player have a combat stance while stationary in combat
                TweenAnimationLayerWeight(legsCombatLayerIndex, legsCombatLayerWeight);
                wasMoving = false;
            }
        }

        private void TweenAnimationLayerWeight(int layerIndex, float targetWeight)
        {
            DOTween.To(
                () => animator.GetLayerWeight(layerIndex),
                value => animator.SetLayerWeight(layerIndex, value),
                targetWeight,
                layerWeightTweenDuration
            );
        }
    }
}
