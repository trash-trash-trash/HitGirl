using UnityEngine;

public class SpawnState: NPCAnthillStateBase
{
    public override void Enter()
    {
        base.Enter();
        //change hp to full
        
        scenarioBrain.hp.ChangeHP(1);
        
        scenarioBrain.awake = true;
        scenarioBrain.spawned = true;
    }

    public override void Exit()
    {
        base.Exit();
        // Cleanup or transition-related logic can go here if needed
    }
}
