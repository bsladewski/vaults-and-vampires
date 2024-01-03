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

    private void Start()
    {
        playerCharacterController.OnPlayerJumped += OnPlayerJumped;
        playerCharacterController.OnPlayerLanded += OnPlayerLanded;
    }

    private void Update()
    {
        float speedNormalized = playerCharacterController.GetSpeedNormalized();
        moveAnimationSpeed = Mathf.Lerp(
            moveAnimationSpeed,
            speedNormalized,
            Time.deltaTime * moveAnimationAcceleration);
        animator.SetFloat("Speed", moveAnimationSpeed);
    }

    private void OnPlayerJumped()
    {
        animator.ResetTrigger("Land");
        animator.SetTrigger("Jump");
    }

    private void OnPlayerLanded()
    {
        animator.SetTrigger("Land");
    }
}
