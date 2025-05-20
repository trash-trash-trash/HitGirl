using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] private PatrolPoint currentPoint;
    [SerializeField] private List<PatrolPoint> patrolPoints;
    [SerializeField] private float arrivalThreshold = 0.5f;

    [SerializeField] private NavMeshAgent agent;
    private int currentPointIndex = 0;

    public bool patrolling = false;

    public void FlipPatrolling(bool input)
    {
        patrolling = input;
    }
    
    public void StartPatrol(List<PatrolPoint> newPatrolPoints)
    {
        patrolPoints = newPatrolPoints;
        currentPointIndex = 0;
        currentPoint = patrolPoints[currentPointIndex];
        agent.SetDestination(currentPoint.transform.position);
        patrolling = true;
    }

    private void FixedUpdate()
    {
        //make better later
        if (!patrolling || !agent.enabled)
            return;
        if (agent.remainingDistance <= arrivalThreshold && !agent.pathPending)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        agent.SetDestination(patrolPoints[currentPointIndex].transform.position);
    }
}