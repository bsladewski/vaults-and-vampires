using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;

    private Transform nextWaypoint;

    private int nextWaypointIndex;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private Ease moveEase = Ease.InOutCubic;

    private void Awake()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints on moving platform is empty!");
        }
    }

    private void Start()
    {
        nextWaypointIndex = 0;
        transform.position = waypoints[nextWaypointIndex].position;
        MovePlatform();
    }

    private void MovePlatform()
    {
        nextWaypoint = GetNextWaypoint();

        float moveDuration = Vector3.Distance(transform.position, nextWaypoint.position) / moveSpeed;
        transform.DOMove(nextWaypoint.position, moveDuration).SetEase(moveEase).onComplete = () =>
        {
            MovePlatform();
        };
    }

    private Transform GetNextWaypoint()
    {
        if (nextWaypointIndex + 1 == waypoints.Length)
        {
            nextWaypointIndex = 0;
        }
        else
        {
            nextWaypointIndex = nextWaypointIndex + 1;
        }

        return waypoints[nextWaypointIndex];
    }
}
