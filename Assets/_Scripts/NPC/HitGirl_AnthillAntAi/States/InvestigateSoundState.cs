using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InvestigateSoundState : NPCAnthillStateBase
{
    [SerializeField] private float arrivalThreshold = 5f;
    private bool hasStartedLookAround = false;

    public float noiseRandomSphere=5;

    public MemoryData mostRecentSound;

    private float originalDecayTime;
    
    public override void Enter()
    {
        base.Enter();
        hasStartedLookAround = false;
        if(!scenarioBrain.idle)
            scenarioBrain.patrol.FlipPatrolling(false);

        StartCoroutine(DelayReaction());
    }
    
    IEnumerator DelayReaction()
    {
        float delay = Random.Range(0.0f, 1.2f);
        yield return new WaitForSeconds(delay);

        CheckSoundCloseFar();
    }

    private void CheckSoundCloseFar()
    {
        scenarioBrain.navMeshAgent.isStopped = false;
        if(scenarioBrain.memory.GetMostRecentMemoryOfType(MemoryEnum.Sound, out MemoryData mem))
        {
            mostRecentSound = mem;
            originalDecayTime = mem.soundData.soundDecayTime;
            if (!mem.soundData.closeSound)
            {  
                //actively walk to the sound
                StartCoroutine(InvestigateRoutine());
                mem.decayTime = 60;
            }

            else
            {
                //turn and look, then resume patrol
                StartCoroutine(DecaySound());
            }
        }
    }
    
    IEnumerator DecaySound()
    {
        yield return new WaitForFixedUpdate();
        
        scenarioBrain.npcHeadLook.FlipLookingAt(mostRecentSound.soundData.originObj.position, true);
        scenarioBrain.debugText.SetText("HEARD SOMETHING?");

        yield return new WaitForSeconds(mostRecentSound.decayTime);

        CheckSoundCloseFar();
    }
    
    private IEnumerator InvestigateRoutine()
    {
        //find random spot for multple npcs investigating the same noise
        Vector3 randomOffset = Random.insideUnitSphere * noiseRandomSphere;
        randomOffset.y = 0; 
        Vector3 targetPosition = mostRecentSound.position + randomOffset;

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(targetPosition, out hit, noiseRandomSphere, UnityEngine.AI.NavMesh.AllAreas))
        {
            targetPosition = hit.position; 
            
            scenarioBrain.navMeshAgent.SetDestination(targetPosition);
            scenarioBrain.npcHeadLook.FlipLookingAt(mostRecentSound.position, false);
            scenarioBrain.debugText.SetText("INVESTIGATE SOUND");
            yield return new WaitForFixedUpdate();
        
            //wait until agent arrives at destination
            while (scenarioBrain.navMeshAgent.pathPending ||
                   scenarioBrain.navMeshAgent.remainingDistance > arrivalThreshold)
                yield return null;
            
            scenarioBrain.debugText.SetText("LOOKING AROUND");
            scenarioBrain.navMeshAgent.isStopped = true;
            
            scenarioBrain.npcHeadLook.LookAround();
            
            yield return new WaitUntil(() => !scenarioBrain.npcHeadLook.lookingAround);
            
            //probably shouldnt reset sounds here
            mostRecentSound.decayTime = 1;

            float delay = Random.Range(1.2f, 1.8f);
            yield return new WaitForSeconds(delay);

            CheckSoundCloseFar();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
        
        if(scenarioBrain.navMeshAgent.isOnNavMesh)
            scenarioBrain.navMeshAgent.isStopped = false;
    }
}
