using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{


    public int Health
    {
        get
        {
            return health;
        }

    }
    public NavMeshAgent agent;
    public float distanceTowaypoint = 1;
    public Transform target;
    public Transform waypointParent;
    public bool loop;
    private bool pinPong = false;
    private Transform[] waypoints;
    private int currentIndex = 1;
    private int health = 100;


    void Start()
    {
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            if(currentIndex >= waypoints.Length)
            {
                if (loop)
                {
                    currentIndex = 1;
                }
                else
                {
                    currentIndex = waypoints.Length - 1;
                    pinPong = true;
                }
            }
            if (currentIndex <= 0)
            {
                if (loop)
                {
                    currentIndex = 1;
                }
                else
                {
                    currentIndex = waypoints.Length - 1;
                    pinPong = false;
                }
            }
            Transform point = waypoints[currentIndex];

            float distance = Vector3.Distance(transform.position, point.position);
            if(distance <= distanceTowaypoint)
            {
                if (pinPong)
                {
                    currentIndex--;
                }
                else
                {
                    currentIndex++;
                }
            }
            agent.SetDestination(point.position);
        }
    }
    public void DealDamage(int damageDealt)
    {
        health -= damageDealt;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
