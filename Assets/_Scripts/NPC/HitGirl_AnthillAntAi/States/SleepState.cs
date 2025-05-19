using UnityEngine;

public class SleepState : NPCAnthillStateBase
{
    public override void Enter()
    {
        base.Enter();
        scenarioBrain.navMeshAgent.enabled = false;
        scenarioBrain.debugText.SetText("ZZZ");
    }
}
