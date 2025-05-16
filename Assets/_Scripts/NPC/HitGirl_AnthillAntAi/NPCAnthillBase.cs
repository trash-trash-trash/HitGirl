using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnthillBase : MonoBehaviour, ISense
{
    public CharacterBase characterBase;
    public Health hp;
    public NavMeshAgent navMeshAgent;
    public NPCHeadLook npcHeadLook;
    public PatrolPaths patrolPaths;
    public Patrol patrol;
    public Sight sight;
    public Sound sound;

    public DebugStateText debugText;
    
    public bool spawned = false;
    public bool awake = false;
    public bool susMid = false;
    public bool susSeenPlayer = false;
    public bool alert = false;
    public bool doJob = false;
    public bool idle = false;
    public bool shoved = false;
    public bool heardSound = false;

    void Start()
    {
        sight.AnnounceCanSeeCharacter += CheckCharacter;
        sight.AnnounceCanSeePlayer += Alert;
        characterBase.AnnounceShoved += Shoved;
        characterBase.AnnounceSoundHeard += SoundHeard;
    }

    private void CheckCharacter(CharacterBase obj)
    {
        if (!obj.hp.Alive)
            alert = true;
    }

    private void Alert(bool obj)
    {
        alert = obj;
    }

    private void Shoved()
    {
        shoved = true;
    }
    
    private void SoundHeard(SoundData obj)
    {
        heardSound = true;
    }

    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
            aWorldState.Set(NPCBaseScenario.Spawned, spawned);
            aWorldState.Set(NPCBaseScenario.Alive, hp.Alive);
            aWorldState.Set(NPCBaseScenario.Awake, awake);
            aWorldState.Set(NPCBaseScenario.SusMid, susMid);
            aWorldState.Set(NPCBaseScenario.SusSeenPlayer, susSeenPlayer);
            aWorldState.Set(NPCBaseScenario.Alert, alert);
            aWorldState.Set(NPCBaseScenario.DoingJob, doJob);
            aWorldState.Set(NPCBaseScenario.Idle, idle);
            aWorldState.Set(NPCBaseScenario.Shoved, shoved);
            aWorldState.Set(NPCBaseScenario.HeardSound, heardSound);
    }
}
