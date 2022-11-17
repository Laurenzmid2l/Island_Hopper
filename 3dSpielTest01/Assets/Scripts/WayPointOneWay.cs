using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointOneWay : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    [SerializeField] GameObject player;
    [SerializeField] float speed = 1f;
    int npoint = 0;

    void Update()
    {
        if (player.GetComponent<PlayerMovment>().ButtonIsGreen())
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[npoint].transform.position, speed * Time.deltaTime);
            Debug.Log("Up");
        }
        
    }
}
