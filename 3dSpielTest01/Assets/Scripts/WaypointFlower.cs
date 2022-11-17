using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFlower : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float speed = 1f;
    int npoint = 0;

    void Update()
    {
        if(Vector3.Distance(transform.position, waypoints[npoint].transform.position) < .1f)
        {
            npoint++;
            if(npoint>=waypoints.Length)
            {
                npoint = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[npoint].transform.position, speed * Time.deltaTime);
    }
}
