using Anthill.AI;
using UnityEngine;

public class PlayerStateBase : AntAIState
{
   public PlayerBrain playerBrain;

   public virtual void Create(GameObject aGameObject)
   {
      base.Create(aGameObject);
      playerBrain = GetComponentInParent<PlayerBrain>();
   }

   public virtual void Enter()
   {
      base.Enter();
   }
}
