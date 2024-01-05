using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Required]
    [SerializeField]
    private PlayerCharacterController playerCharacterController;

    [Required]
    [SerializeField]
    private ThirdPersonMovement thirdPersonMovement;

    [Required]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float moveAnimationAcceleration = 20f;

    private float moveAnimationSpeed;

    private float horizontalAnimationSpeed;

    private bool wasAimLocked;

    private void Start()
    {
        playerCharacterController.OnPlayerJumped += OnPlayerJumped;
        playerCharacterController.OnPlayerFell += OnPlayerFell;
        playerCharacterController.OnPlayerLanded += OnPlayerLanded;
    }

    private void LateUpdate()
    {
        bool isAimLocked = thirdPersonMovement.GetIsAimLocked();
        if (isAimLocked != wasAimLocked)
        {
            wasAimLocked = isAimLocked;
            animator.SetBool("Is Aim Locked", isAimLocked);
        }

        if (isAimLocked)
        {
            HandleAimLockedMovementAnimations();
        }
        else
        {
            HandleMovementAnimations();
        }
    }

    private void HandleMovementAnimations()
    {
        float speedNormalized = thirdPersonMovement.GetMovementDirection().magnitude;
        moveAnimationSpeed = Mathf.Lerp(
            moveAnimationSpeed,
            speedNormalized,
            Time.deltaTime * moveAnimationAcceleration);
        animator.SetFloat("Speed", moveAnimationSpeed);
    }

    private void HandleAimLockedMovementAnimations()
    {
        Vector3 movementDirection = thirdPersonMovement.GetMovementDirection();
        moveAnimationSpeed = Mathf.Lerp(
            moveAnimationSpeed,
            movementDirection.x,
            Time.deltaTime * moveAnimationAcceleration
        );
        horizontalAnimationSpeed = Mathf.Lerp(
            horizontalAnimationSpeed,
            movementDirection.z,
            Time.deltaTime * moveAnimationAcceleration
        );
        animator.SetFloat("Speed", moveAnimationSpeed);
        animator.SetFloat("Horizontal Speed", horizontalAnimationSpeed);
    }

    private void OnPlayerJumped()
    {
        ResetTriggers();
        animator.SetTrigger("Jump");
    }

    private void OnPlayerFell()
    {
        ResetTriggers();
        animator.SetTrigger("Fall");
    }

    private void OnPlayerLanded()
    {
        ResetTriggers();
        animator.SetTrigger("Land");
    }

    private void ResetTriggers()
    {
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Fall");
        animator.ResetTrigger("Land");
    }
}