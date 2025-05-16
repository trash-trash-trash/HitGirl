using UnityEngine;

public class Player : CharacterBase
{
   //character base inherits from anthill. split up

   [SerializeField]
   private bool aggroAction;

   public bool AggroAction
   {
      get { return aggroAction; }
      set { aggroAction = value; }
   }
}
