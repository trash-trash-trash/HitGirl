using UnityEngine;
using UnityEngine.AI;

public class AlertState : NPCAnthillStateBase
{
    public float speed;

    public NavMeshAgent myAgent;
    public PatrolPoint fleePoint;

    public Vector3 startingPoint;
    
    public override void Enter()
    {
        base.Enter();
        scenarioBrain.debugText.SetText("!");
        myAgent = scenarioBrain.navMeshAgent;
        startingPoint = myAgent.transform.position;
        speed = myAgent.speed;
        myAgent.speed = speed * 2;
        
        fleePoint = scenarioBrain.patrolPaths.fleePatrolPoints[
            Random.Range(0, scenarioBrain.patrolPaths.fleePatrolPoints.Count)
        ];
        scenarioBrain.npcHeadLook.FlipLookingAt(Vector3.back, false);
    }
    
    private void FixedUpdate()
    {
        if (fleePoint != null && myAgent != null)
        {
            myAgent.SetDestination(fleePoint.transform.position);
        }
        //default arrival threshold 0.5
        if (!myAgent.pathPending && myAgent.remainingDistance <= 0.5f)
        {
            scenarioBrain.npcHeadLook.FlipLookingAt(startingPoint, true);
        }
    }
}
