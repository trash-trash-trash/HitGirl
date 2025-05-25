using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour, IInteract
{
    //make this the generic pathfinding navmesh for all states to use
    
    [SerializeField] private PatrolPoint currentPoint;
    [SerializeField] private List<PatrolPoint> patrolPoints;
    public float currentDistFromTarget;
    [SerializeField] private float patrolPointArrivalThreshold;
    [SerializeField] private float doorArrivalThreshold;

    [SerializeField] private NavMeshAgent agent;
    private int currentPointIndex = 0;

    public bool patrolling = false;
    public bool openingDoor = false;

    public Door nextDoor;
    public Door prevDoor;
    private bool hasOpenedDoor = false;

    public void FlipPatrolling(bool input)
    {
        patrolling = input;
    }
    
    public void StartPatrol(List<PatrolPoint> newPatrolPoints)
    {
        patrolPoints = newPatrolPoints;
        currentPointIndex = 0;
        currentPoint = patrolPoints[currentPointIndex];
        CalculatePath();
        patrolling = true;
    }

    void CalculatePath()
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(currentPoint.transform.position, path);
        IInteractable blockingDoor = InteractableRegistry.FindBlockingDoor(path);

        if (blockingDoor != null)
        {
            GameObject doorObj = blockingDoor.ReturnSelf();
            nextDoor = doorObj.GetComponent<Door>();
        }

        if (nextDoor != null && !nextDoor.open)
        {
            openingDoor = true;
            Debug.Log("door in my way");
            agent.SetDestination(nextDoor.ReturnSelf().transform.position);
        }
        else
            agent.SetDestination(currentPoint.transform.position);
    }

    private void FixedUpdate()
    {
        if (agent.hasPath)
            currentDistFromTarget = agent.remainingDistance;
        
        //make better later
        if (!patrolling || !agent.enabled)
            return;
        
        if (hasOpenedDoor && prevDoor != null)
        {
            float distance = Vector3.Distance(agent.transform.position, prevDoor.transform.position);
            if (distance >= doorArrivalThreshold && prevDoor.open)
            {
                prevDoor.Interact(this, CharacterActions.Use);
                hasOpenedDoor = false;
                prevDoor = null;
            }
        }
        
        if (agent.remainingDistance <= patrolPointArrivalThreshold && !agent.pathPending)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    { 
        if (openingDoor)
             {
                 if (!nextDoor.open)
                 {
                     nextDoor.Interact(this, CharacterActions.Use);
                     prevDoor = nextDoor;
                     hasOpenedDoor = true;
                     CalculatePath();
                 }

                 else if (nextDoor.open)
                 {
                     openingDoor = false;
                     CalculatePath();
                 }
             }
        else
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            currentPoint = patrolPoints[currentPointIndex];
            CalculatePath();
        }

    }
}