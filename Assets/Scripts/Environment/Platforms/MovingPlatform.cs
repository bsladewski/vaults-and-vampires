using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using DG.Tweening;
using Utils;

namespace Environment
{
    [SelectionBase]
    public class MovingPlatform : MonoBehaviour, IMoverController
    {
        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private PhysicsMover mover;

        [SerializeField]
        private Waypoints waypoints;

        [Header("Settings")]
        [SerializeField]
        private Ease defaultWaypointEase = Ease.InOutQuad;

        [SerializeField]
        private float moveSpeed = 2f;

        private int currentWaypointIndex;

        private Vector3 targetPosition;

        private Vector3 targetRotation;

        private Vector3 initialPosition;

        private void Awake()
        {
            mover.MoverController = this;
            initialPosition = transform.position;
        }

        private void Start()
        {
            if (waypoints != null && waypoints.GetLength() <= 1)
            {
                // waypoint based movement requires at least 2 waypoints
                Debug.LogError("MovingPlatform waypoints should be >1!");
            }

            if (waypoints != null && waypoints.GetLength() > 1)
            {
                // if waypoints were specified, move the platform to the first waypoint and start movement
                targetPosition = waypoints.GetWaypoint(0).position + initialPosition;
                mover.Rigidbody.position = targetPosition;
                if (waypoints.GetIsCircuit())
                {
                    HandleCircuitWaypointMovement();
                }
                else
                {
                    HandleOneWayWaypointMovement();
                }
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
            goalRotation = Quaternion.Euler(targetRotation);
        }

        private void HandleCircuitWaypointMovement()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.GetLength();
            Tween tween = HandleWaypointMovement();
            tween.onComplete = () => HandleCircuitWaypointMovement();
        }

        private void HandleOneWayWaypointMovement()
        {
            if (currentWaypointIndex >= waypoints.GetLength() - 1)
            {
                // if we've reached the end of the path, begin traversal backwards
                currentWaypointIndex = waypoints.GetLength() - 1;
                HandleOneWayWaypointMovementBackwards();
                return;
            }

            currentWaypointIndex++;
            Tween tween = HandleWaypointMovement();
            tween.onComplete = () => HandleOneWayWaypointMovement();
        }

        private void HandleOneWayWaypointMovementBackwards()
        {
            if (currentWaypointIndex <= 0)
            {
                // if we've reached the start of the path, begin normal traversal
                currentWaypointIndex = 0;
                HandleOneWayWaypointMovement();
                return;
            }

            currentWaypointIndex--;
            Tween tween = HandleWaypointMovement();
            tween.onComplete = () => HandleOneWayWaypointMovementBackwards();
        }

        private Tween HandleWaypointMovement()
        {
            Waypoint waypoint = waypoints.GetWaypoint(currentWaypointIndex);
            Vector3 nextPosition = waypoint.position + initialPosition;
            Vector3 nextRotation = waypoint.rotation;
            Ease waypointEase = waypoint.ease == Ease.Unset ? defaultWaypointEase : waypoint.ease;
            DOTween.To(
                () => targetRotation,
                value => targetRotation = value,
                nextRotation,
                Vector3.Distance(targetPosition, nextPosition) / moveSpeed
            ).SetEase(waypointEase).SetDelay(waypoint.delay);
            return DOTween.To(
                () => targetPosition,
                value => targetPosition = value,
                nextPosition,
                Vector3.Distance(targetPosition, nextPosition) / moveSpeed
            ).SetEase(waypointEase).SetDelay(waypoint.delay);
        }
    }
}
