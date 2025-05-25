using System.Collections;
using UnityEngine;

public class WakeupState : NPCAnthillStateBase
{
    [SerializeField] private float wakeupDistance = 3f;
    [SerializeField] private float distance;
    private bool hasWokenUp = false;

    public CharacterBase sleepingChar;
    MemoryData mem;
    
    private Vector3 lastTargetPosition;
    private float repathThreshold = 0.1f;

    public float wakeupChannelTime = 3f;
    
    public Coroutine wakeupCoroutine;
    
    public override void Enter()
    {
        base.Enter();
        hasWokenUp = false;
        
        if (scenarioBrain.memory.GetMostRecentMemoryOfType(MemoryEnum.SleepingChar, out mem))
        {
            sleepingChar = mem.character;
            scenarioBrain.npcHeadLook.FlipLookingAt(sleepingChar.transform.position, true);
            scenarioBrain.patrol.FlipPatrolling(false);
        }
        else
        {
            hasWokenUp = true;
        }
        
        StartCoroutine(DelayReaction());
    }

    IEnumerator DelayReaction()
    {
        float delay = Random.Range(0.0f, 1.2f);
        yield return new WaitForSeconds(delay);
        scenarioBrain.navMeshAgent.SetDestination(sleepingChar.transform.position);
    }

    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        base.Execute(aDeltaTime, aTimeScale);  
        
        if (sleepingChar.hp._healthStatus == HealthStatus.Fine)
            hasWokenUp = true;

        if(hasWokenUp)
          if (scenarioBrain.memory.GetMostRecentMemoryOfType(MemoryEnum.SleepingChar, out MemoryData mem))
          {
            sleepingChar = mem.character;
            hasWokenUp = false;
          }

        if (hasWokenUp || sleepingChar == null || !scenarioBrain.navMeshAgent.enabled)
            return;
        
        scenarioBrain.npcHeadLook.FlipLookingAt(sleepingChar.transform.position, true);
        
        Vector3 currentTargetPosition = sleepingChar.transform.position;
        if (Vector3.Distance(currentTargetPosition, lastTargetPosition) > repathThreshold)
        {
            scenarioBrain.navMeshAgent.SetDestination(currentTargetPosition);
            lastTargetPosition = currentTargetPosition;
        }

        distance = Vector3.Distance(scenarioBrain.transform.position, sleepingChar.transform.position);

        if (!scenarioBrain.navMeshAgent.pathPending && scenarioBrain.navMeshAgent.remainingDistance <= wakeupDistance)
        {
            scenarioBrain.navMeshAgent.isStopped = true;
            wakeupCoroutine = StartCoroutine(WakeUpCharacter());
        }
        
    }

    private IEnumerator WakeUpCharacter()
    {
        float countdown = wakeupChannelTime;

        while (countdown > 0f)
        {
            //stop if character woke up early, ie by someone else
            if (sleepingChar == null || sleepingChar.hp._healthStatus == HealthStatus.Fine)
            {
                hasWokenUp = true;
                wakeupCoroutine = null;
                yield break;
            }

            yield return new WaitForFixedUpdate();
            countdown -= Time.fixedDeltaTime;
        }

        if (sleepingChar != null && sleepingChar.hp._healthStatus != HealthStatus.Fine)
        {
            IInteractable interactable = sleepingChar.GetComponent<IInteractable>();
            interactable?.Interact(scenarioBrain.patrol, CharacterActions.WakeUp);
        }

        scenarioBrain.npcHeadLook.FlipLookingAt(sleepingChar.gameObject.transform.position, true);
        StartCoroutine(DelayReactionWakeup());
    }

    IEnumerator DelayReactionWakeup()
    {
        float delay = Random.Range(1f, 2.5f);
        yield return new WaitForSeconds(delay);
        hasWokenUp = true;
        wakeupCoroutine = null;
        scenarioBrain.navMeshAgent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
        hasWokenUp = false;
        scenarioBrain.navMeshAgent.isStopped = false;
        if (wakeupCoroutine != null)
        {
            StopCoroutine(wakeupCoroutine);
            wakeupCoroutine = null;
        }
    }
}
