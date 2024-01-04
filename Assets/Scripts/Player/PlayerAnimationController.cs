using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Required]
    [SerializeField]
    private PlayerCharacterController playerCharacterController;

    [Required]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float moveAnimationAcceleration = 20f;

    private float moveAnimationSpeed;

    private bool isJumping;

    private void Start()
    {
        playerCharacterController.OnPlayerJumped += OnPlayerJumped;
        playerCharacterController.OnPlayerFell += OnPlayerFell;
        playerCharacterController.OnPlayerLanded += OnPlayerLanded;
    }

    private void LateUpdate()
    {
        float speedNormalized = playerCharacterController.GetSpeedNormalized();
        moveAnimationSpeed = Mathf.Lerp(
            moveAnimationSpeed,
            speedNormalized,
            Time.deltaTime * moveAnimationAcceleration);
        animator.SetFloat("Speed", moveAnimationSpeed);
        if (isJumping && playerCharacterController.GetIsGrounded())
        {
            // extra check to keep us from getting stuck in the jump state
            OnPlayerLanded();
        }
    }

    private void OnPlayerJumped()
    {
        OnPlayerFell();
    }

    private void OnPlayerFell()
    {
        animator.ResetTrigger("Land");
        animator.SetTrigger("Jump");
        isJumping = true;
    }

    private void OnPlayerLanded()
    {
        animator.ResetTrigger("Jump");
        animator.SetTrigger("Land");
        isJumping = false;
    }
}
