using UnityEngine;
using UnityEngine.AI;

public class IdleState : NPCAnthillStateBase
{
    private bool hasArrived = false;

    private PatrolPoint myIdlePoint;
    private NavMeshAgent myAgent;
    
    public override void Enter()
    {
        base.Enter();
        hasArrived = false;
        myIdlePoint = scenarioBrain.patrolPaths.idlePoint;
        myAgent = scenarioBrain.navMeshAgent;
        scenarioBrain.npcHeadLook.FlipLookingAt(Vector3.zero, false);
        scenarioBrain.debugText.SetText("Returning to Idle");
        
        myAgent.SetDestination(myIdlePoint.transform.position);
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        base.Execute(aDeltaTime, aTimeScale);
        if (!myAgent.enabled)
            return;
        
        if (!myAgent.pathPending && myAgent.remainingDistance <= 0.5f)
        {
            OnIdlePointReached();
        }
    }

    private void OnIdlePointReached()
    {
        scenarioBrain.debugText.SetText("Idle");
        hasArrived = true;
        scenarioBrain.npcHeadLook.FlipLookingAt(myIdlePoint.pointOfInterest.position, true);
    }
}
