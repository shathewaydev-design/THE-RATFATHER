using UnityEngine;

public class EnforcerPath : MonoBehaviour
{

    //----------global variables----------
    public enum PathType
    {
        Loop,
        ReverseWhenComplete
    }

    public Transform[] waypoints;
    public PathType pathType = PathType.Loop;

    private int direction = 1;
    int index;


    //----------main methods----------
    public Vector3 GetCurrentWaypoint()
    {
        return waypoints[index].position;
    }

    public Vector3 GetNextWaypoint() // based on pathtype, retrieve next position
    {
        if (waypoints.Length == 0) return transform.position;

        index = GetNextWaypointIndex();
        Vector3 nextWaypoint = waypoints[index].position;

        return nextWaypoint;
    }


    // ----------helpers----------
    private int GetNextWaypointIndex()
    {
        // move to next waypoint
        index += direction;
     
        if (pathType == PathType.Loop) 
        {
            index %= waypoints.Length;
        }
        else if (pathType == PathType.ReverseWhenComplete)
        {
            if (index >= waypoints.Length || index < 0) 
            {
                direction *= -1;
                index += direction * 2;

            }

        }

        return index;
    }
















    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
