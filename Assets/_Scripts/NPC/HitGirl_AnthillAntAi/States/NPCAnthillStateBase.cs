using Anthill.AI;
using UnityEngine;

public class NPCAnthillStateBase : AntAIState
{
    protected NPCAnthillBase scenarioBrain;

    public override void Create(GameObject aGameObject)
    {
        base.Create(aGameObject);
        scenarioBrain = aGameObject.GetComponentInParent<NPCAnthillBase>();
    }
        
    public override void Enter()
    {
        base.Enter(); 
        scenarioBrain.debugText.SetText(name);
    }
        
    public override void Exit() 
    { 
        base.Exit(); 
    }
}
