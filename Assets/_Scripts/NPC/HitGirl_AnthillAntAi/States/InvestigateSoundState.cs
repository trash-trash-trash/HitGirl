using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InvestigateSoundState : NPCAnthillStateBase
{
    [SerializeField] private float arrivalThreshold = 5f;
    private float originalSpeed;
    private bool hasStartedLookAround = false;

    public float noiseRandomSphere=5;

    public MemoryData mostRecentSound;
    
    public override void Enter()
    {
        base.Enter();
        hasStartedLookAround = false;
        originalSpeed = scenarioBrain.navMeshAgent.speed;
        if(!scenarioBrain.idle)
            scenarioBrain.patrol.FlipPatrolling(false);
        
        CheckSoundCloseFar();
    }

    private void CheckSoundCloseFar()
    {
        if(scenarioBrain.memory.GetMostRecentMemoryOfType(MemoryEnum.Sound, out MemoryData mem))
        {
            mostRecentSound = mem;
            if (mem.soundData.closeSound)
            {  
                //actively walk to the sound
                StartCoroutine(InvestigateRoutine());
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
        scenarioBrain.navMeshAgent.speed = 0;
        scenarioBrain.npcHeadLook.FlipLookingAt(mostRecentSound.soundData.originObj.position, true);
        scenarioBrain.debugText.SetText("HEARD SOMETHING?");

        yield return new WaitForSeconds(mostRecentSound.decayTime);
        scenarioBrain.navMeshAgent.speed = originalSpeed;

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
            {
                yield return null;
            }

            //silly improve
            scenarioBrain.debugText.SetText("LOOKING AROUND");
            Vector3 lookLeft = new Vector3(transform.position.x, transform.position.y - 65,
                transform.position.z);
            Vector3 lookRight  = new Vector3(transform.position.x, transform.position.y + 65,
                transform.position.z);

            bool lookLeftFirst = Random.value < 0.5f;
        
            scenarioBrain.npcHeadLook.FlipLookingAt(lookLeftFirst ? lookLeft : lookRight, true);

            yield return new WaitForSeconds(3f);

            scenarioBrain.npcHeadLook.FlipLookingAt(lookLeftFirst ?  lookRight : lookLeft, true);
        
            yield return new WaitForSeconds(3.5f);
            
            CheckSoundCloseFar();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
        scenarioBrain.navMeshAgent.speed = originalSpeed;
    }
}
