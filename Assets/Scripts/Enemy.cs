using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Seek
    }
    public int Health
    {
        get
        {
            return health;
        }

    }
    public State currentState = State.Patrol;
    public NavMeshAgent agent;
    public float distanceToWaypoint = 1;
    public Transform target;
    public Transform waypointParent;
    public bool loop;
    private bool pinPong = false;
    private Transform[] waypoints;
    private int currentIndex = 1;
    private int health = 100;
    public float detectionRadius = 10;
    public FieldOfView fov;
    public AudioSource alertSound;
    public GameObject alertSymbol;
  
    IEnumerator SeekDelay()
    {
        yield return new WaitForSeconds(1f);
        // Switch to 'Seek' state
        currentState = State.Patrol;
        target = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    void Start()
    {
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Seek:
                Seek();
                break;
            default:
                break;
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
    void Patrol()
    {
        alertSymbol.SetActive(false);
        // If the currentIndex is out of waypoint range
        if (currentIndex >= waypoints.Length)
        {
            // Go back to "first" (actually second) waypoint
            currentIndex = 1;
        }
        // Set the current waypoint
        Transform point = waypoints[currentIndex];
        // Get distance to waypoint
        float distance = Vector3.Distance(transform.position, point.position);
        // If waypoint is within range
        if (distance <= distanceToWaypoint)
        {
            // Move to next waypoint (Next Frame)
            currentIndex++;
        }
        // Generate path to current waypoint
        agent.SetDestination(point.position);
        if (fov.visibleTargets.Count > 0)
        {
            target = fov.visibleTargets[0];
            currentState = State.Seek;
            alertSound.Play(); //Audiosource
            alertSymbol.SetActive(true); // TextMeshPro = Needs plugin
        }
    }
    void Seek()
    {
        agent.SetDestination(target.position);
        // Get distance to target
        float distToTarget = Vector3.Distance(transform.position, target.position);
        // If the target is within detection range
        if (distToTarget >= detectionRadius)
        {
            StartCoroutine(SeekDelay());
        }
    }
}