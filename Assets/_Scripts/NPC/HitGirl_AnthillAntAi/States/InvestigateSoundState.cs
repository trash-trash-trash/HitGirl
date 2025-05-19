using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InvestigateSoundState : NPCAnthillStateBase
{
    public SoundData soundHeard;

    [SerializeField] private float arrivalThreshold = 5f;
    public bool reallyInvestigating=false;
    private float originalSpeed;
    private bool hasStartedLookAround = false;

    public float noiseRandomSphere=5;
    
    public override void Enter()
    {
        base.Enter();
        hasStartedLookAround = false;
        originalSpeed = scenarioBrain.navMeshAgent.speed;
        if(!scenarioBrain.idle)
            scenarioBrain.patrol.FlipPatrolling(false);
        
        soundHeard = scenarioBrain.characterBase.mostRecentSoundHeard;
        if (soundHeard.closeSound)
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
    
    IEnumerator DecaySound()
    {
        yield return new WaitForFixedUpdate();
        scenarioBrain.navMeshAgent.speed = 0;
        scenarioBrain.npcHeadLook.FlipLookingAt(soundHeard.originObj.position, true);
        scenarioBrain.debugText.SetText("HEARD SOMETHING?");

        yield return new WaitForSeconds(soundHeard.decayTime);
        scenarioBrain.navMeshAgent.speed = originalSpeed;
        scenarioBrain.heardSound = false;
    }
    
    private IEnumerator InvestigateRoutine()
    {
        //find random spot for multple npcs investigating the same noise
        Vector3 randomOffset = Random.insideUnitSphere * noiseRandomSphere;
        randomOffset.y = 0; 
        Vector3 targetPosition = soundHeard.soundOrigin + randomOffset;

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(targetPosition, out hit, noiseRandomSphere, UnityEngine.AI.NavMesh.AllAreas))
        {
            targetPosition = hit.position; 
            
            scenarioBrain.navMeshAgent.SetDestination(targetPosition);
            scenarioBrain.npcHeadLook.FlipLookingAt(soundHeard.originObj.position, false);
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
            Vector3 lookLeft = new Vector3(soundHeard.originObj.position.x, soundHeard.originObj.position.y - 65,
                soundHeard.originObj.position.z);
            Vector3 lookRight  = new Vector3(soundHeard.originObj.position.x, soundHeard.originObj.position.y - 65,
                soundHeard.originObj.position.z);

            bool lookLeftFirst = Random.value < 0.5f;
        
            scenarioBrain.npcHeadLook.FlipLookingAt(lookLeftFirst ? lookLeft : lookRight, true);

            yield return new WaitForSeconds(3f);

            scenarioBrain.npcHeadLook.FlipLookingAt(lookLeftFirst ?  lookRight : lookLeft, true);
        
            yield return new WaitForSeconds(3.5f);
        
            scenarioBrain.heardSound = false; 
        }
        else
        {
            StartCoroutine(DecaySound());
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
}
