using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using DG.Tweening;

public class KinematicMovingPlatform : MonoBehaviour, IMoverController
{
    [Required]
    [SerializeField]
    private PhysicsMover mover;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private Ease waypointEase = Ease.InOutQuad;

    [SerializeField]
    private float waypointDelay = 1f;

    [SerializeField]
    private Transform[] waypoints;

    private int currentWaypointIndex;

    private Vector3 targetPosition;

    private void Awake()
    {
        mover.MoverController = this;
    }

    private void Start()
    {
        if (waypoints != null && waypoints.Length == 1)
        {
            // waypoint based movement requires at least 2 waypoints
            Debug.LogError("KinematicMovingPlatform waypoints should be empty or >1!");
        }

        if (waypoints != null && waypoints.Length > 1)
        {
            // if waypoints were specified, move the platform to the first waypoint and start movement
            targetPosition = waypoints[0].position;
            mover.Rigidbody.position = targetPosition;
            HandleWaypointMovement();
        }
        else
        {
            // if waypoints were not specified, keep the platform at its current position
            targetPosition = mover.Rigidbody.position;
        }
    }

    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        goalPosition = targetPosition;
        goalRotation = mover.Rigidbody.rotation;
    }

    private void HandleWaypointMovement()
    {
        // get the next waypoint and start a movement tween towards it
        // when the tween is complete it will begin movement towards the subsequent waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        DOTween.To(
            () => targetPosition,
            value => targetPosition = value,
            waypoints[currentWaypointIndex].position,
            Vector3.Distance(targetPosition, waypoints[currentWaypointIndex].position) / moveSpeed
        ).SetEase(waypointEase).SetDelay(waypointDelay).onComplete = () => HandleWaypointMovement();
    }
}
