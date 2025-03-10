using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBallon : MonoBehaviour
{

    public Transform[] waypoints;
    public float speed = 5f;
    private int currentWaypoint = 0;
    public Transform a;
    public Transform b;
    public Transform c;
    public Transform d;

    // Start is called before the first frame update
    void Start()
    {
        waypoints[0] = a;
        waypoints[1] = b;
        waypoints[2] = c;
        waypoints[3] = d;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBalloon();
    }

    void MoveBalloon()
    {
        float journeyFraction = speed * Time.deltaTime / Vector3.Distance(transform.position, waypoints[currentWaypoint].position);

        transform.position = Vector3.Lerp(transform.position, waypoints[currentWaypoint].position, journeyFraction);

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
            
        }
    }
}
