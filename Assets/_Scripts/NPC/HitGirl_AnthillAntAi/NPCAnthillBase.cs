using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnthillBase : MonoBehaviour, ISense, IInteractable, IHear
{
    public CharacterBase characterBase;
    public Clothes clothes;
    public Health hp;
    public Memory memory;
    public NavMeshAgent navMeshAgent;
    public NPCHeadLook npcHeadLook;
    public PatrolPaths patrolPaths;
    public Patrol patrol;
    public Sight sight;
    public Sound sound;
    public ZoneChecker zoneChecker;

    public DebugStateText debugText;
    
    public bool spawned = false;
    public bool awake = false;
    public bool susMid = false;
    public bool susSeenPlayer = false;
    public bool alert = false;
    public bool doJob = false;
    public bool idle = false;
    public bool shoved = false;

    private bool canHear = true;

    public bool CanHear
    {
        get { return canHear; }
        set { canHear = value; }
    }

    void Start()
    {
        sight.AnnounceCanSeeCharacter += CheckCharacter;
        sight.AnnounceCanSeePlayer += SeenPlayer;
        characterBase.AnnounceShoved += Shoved;
        characterBase.AnnounceSlept += Slept;
        hp.AnnounceHealthStatus += SetStatus;
    }

    private void SetStatus(HealthStatus newStatus)
    {
        if (newStatus != HealthStatus.Fine)
        {
            navMeshAgent.enabled = false;
            CanHear = false;
            sight.CanSee = false;  
            
            if (newStatus == HealthStatus.Asleep)
                awake = false;
        }
        else
        {
            awake = true;
            CanHear = true;
            sight.CanSee = true;
        }
    }

    private void Slept()
    {
        awake = false;
    }

    private void CheckCharacter(CharacterBase obj)
    {
        //change here if want to track objects in memory
        if (obj.hp == null)
            return;
            
            if (!obj.hp.Alive)
        {
            //repetition
            MemoryData newMemory = new MemoryData()
            {
                memoryType = MemoryEnum.DeadChar,
                character = obj,
                position = obj.transform.position,
                timeMemoryAdded = Time.time,
                //max?
                decayTime = 30
            };
            
            memory.AddOrUpdateMemory(newMemory);
            if (memory.deadAlly)
                alert = true;
        }
            
        else if (obj.hp._healthStatus == HealthStatus.Asleep)
        { 
            MemoryData newMemory = new MemoryData()
                {
                    memoryType = MemoryEnum.SleepingChar,
                    character = obj,
                    position = obj.transform.position,
                    timeMemoryAdded = Time.time,
                    decayTime = 30
                };
            
                memory.AddOrUpdateMemory(newMemory);
        }
        else
        {
            MemoryData newMemory = new MemoryData()
            {
                memoryType = MemoryEnum.Character,
                character = obj,
                position = obj.transform.position,
                timeMemoryAdded = Time.time,
                decayTime = 3
            };
            
            memory.AddOrUpdateMemory(newMemory);
        }
    }

    private void SeenPlayer(Player player, bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            if (player.AggroAction)
                alert = true;

            if (!zoneChecker.CheckIsPlayerCharacterAllowedInArea(player))
                alert = true;
        }
    }

    private void Shoved()
    {
        shoved = true;
    }
    
    public void HeardSound(SoundData sound)
    {
        if (!CanHear)
            return;

        MemoryData mem = new MemoryData()
        {
            character = sound.soundSource,
            memoryType = MemoryEnum.Sound,
            decayTime = sound.soundDecayTime,
            position = sound.soundOrigin,
            timeMemoryAdded = Time.time,
            soundData = sound
        };
        memory.AddOrUpdateMemory(mem);
    }

    public void FlipCanHear(bool input)
    {
        CanHear = input;
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
            aWorldState.Set(NPCBaseScenario.HeardSound, memory.heardSound);
            aWorldState.Set(NPCBaseScenario.SleepingAlly, memory.sleepingAlly);
    }

    public void Interact(IInteract interactee, CharacterActions actionType)
    {
        //fix
        if (actionType == CharacterActions.Undress)
        {
            if (!awake || !hp.Alive)
                clothes.Undress();
        }
        else if (actionType == CharacterActions.WakeUp)
            if (!awake)
                hp._healthStatus = HealthStatus.Fine;
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }

    private bool canInteract = true;
    
    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }
    public bool ReturnCanInteract()
    {
        return CanInteract;
    }
}
