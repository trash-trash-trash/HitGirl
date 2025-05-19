using UnityEngine;

public class WakeupState : NPCAnthillStateBase
{
    [SerializeField] private float wakeupDistance = 2f;
    [SerializeField] private float distance;
    private bool hasWokenUp = false;
    
    public override void Enter()
    {
        base.Enter();
        hasWokenUp = false;
        scenarioBrain.patrol.FlipPatrolling(false);
    }
    
    private void Update()
    {
        if (hasWokenUp || scenarioBrain.sleepingChar == null)
            return;
        
        scenarioBrain.navMeshAgent.SetDestination(scenarioBrain.sleepingChar.transform.position);

        distance = Vector3.Distance(scenarioBrain.transform.position, scenarioBrain.sleepingChar.transform.position);

        if (distance <= wakeupDistance)
        {
            WakeUpCharacter();
            hasWokenUp = true;
        }
    }

    private void WakeUpCharacter()
    {
        IInteractable interactable = scenarioBrain.sleepingChar.GetComponent<IInteractable>();
        interactable.Interact(CharacterActions.WakeUp);
    }

    private void Exit()
    {
        hasWokenUp = false;
    }
}
