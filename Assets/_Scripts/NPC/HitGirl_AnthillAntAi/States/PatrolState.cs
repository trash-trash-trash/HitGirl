using UnityEngine;

public class PatrolState: NPCAnthillStateBase
{
    public NPCHeadLook npcHeadLook;
    private bool firstTime = true;
    
    public override void Enter()
    {
        base.Enter();
        npcHeadLook = GetComponentInParent<NPCHeadLook>();

        npcHeadLook.FlipLookingAt(Vector3.back, false);

        if (firstTime)
        {
            scenarioBrain.patrol.StartPatrol(scenarioBrain.patrolPaths.idlePatrolPoints);
            firstTime = false;
        }
        
        scenarioBrain.patrol.FlipPatrolling(true);
    }
}
