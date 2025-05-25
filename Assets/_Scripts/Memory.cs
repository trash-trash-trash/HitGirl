using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public Dictionary<CharacterBase, MemoryData> memories = new();

    public List<CharacterBase> knownCharacters = new List<CharacterBase>();
    public List<MemoryData> memoriesList = new List<MemoryData>();

    public bool heardSound = false;
    public bool deadAlly = false;
    public bool sleepingAlly = false;

    public void AddOrUpdateMemory(MemoryData mem)
    {
        // Check if we already have a memory of this character
        if (memories.TryGetValue(mem.character, out var existing))
        {
            // Update only if something has changed
            if (existing.memoryType != mem.memoryType || 
                (mem.character.transform.position - existing.position).sqrMagnitude > 0.01f)
            {
                existing.memoryType = mem.memoryType;
                existing.position = mem.position;
                existing.timeMemoryAdded = Time.time;
                existing.decayTime = mem.decayTime;
            }
        }
        else
        {
            memories[mem.character] = mem;
            knownCharacters.Add(mem.character);
            memoriesList.Add(mem);
        }

        if (mem.memoryType == MemoryEnum.Sound)
            StartCoroutine(DecayMemory(mem));
        
        // Recalculate status flags
        RecalculateStatusFlags();
    }

    IEnumerator DecayMemory(MemoryData mem)
    {
        while (mem.decayTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            mem.decayTime -= 1f;
        }

        RemoveMemory(mem.character);
    }

    public void RemoveMemory(CharacterBase character)
    {
        if (memories.TryGetValue(character, out var memData))
        { 
            memoriesList.Remove(memData);
            memories.Remove(character);
            RecalculateStatusFlags(); 
        }
    }

    private void RecalculateStatusFlags()
    {
        sleepingAlly = memories.Values.Any(mem => mem.memoryType == MemoryEnum.SleepingChar);
        deadAlly = memories.Values.Any(mem => mem.memoryType == MemoryEnum.DeadChar);
        heardSound = memories.Values.Any(mem => mem.memoryType == MemoryEnum.Sound);
    }

    public bool GetMostRecentMemoryOfType(MemoryEnum type, out MemoryData mem)
    {
        mem = null;
        float latestTime = float.MinValue;

        foreach (var memory in memories.Values)
        {
            if (memory.memoryType == type && memory.timeMemoryAdded > latestTime)
            {
                mem = memory;
                latestTime = memory.timeMemoryAdded;
            }
        }

        return mem != null;
    }
}
