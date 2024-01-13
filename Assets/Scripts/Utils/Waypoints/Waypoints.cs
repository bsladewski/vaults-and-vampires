using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField]
    private bool isCircuit;

    [SerializeField]
    private Waypoint[] waypoints;

    public bool GetIsCircuit()
    {
        return isCircuit;
    }

    public Waypoint[] GetWaypoints()
    {
        return waypoints;
    }

    public int GetLength()
    {
        if (waypoints == null)
        {
            return 0;
        }

        return waypoints.Length;
    }

    public Waypoint GetWaypoint(int index)
    {
        return waypoints[index];
    }
}
