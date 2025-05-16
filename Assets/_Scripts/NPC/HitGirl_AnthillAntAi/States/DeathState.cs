using UnityEngine;

public class DeathState : NPCAnthillStateBase
{
    public override void Enter()
    {
        base.Enter();
        scenarioBrain.navMeshAgent.enabled = false;
    }
}

