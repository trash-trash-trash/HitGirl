
using System;
using UnityEngine;

[Serializable]
public class MemoryData
{
    public MemoryEnum memoryType;
    public CharacterBase character;
    public Vector3 position;
    public float timeMemoryAdded;
    public float decayTime;

    public SoundData soundData;
}

[Serializable]
public enum MemoryEnum
{
    SleepingChar,
    DeadChar,
    Character,
    Sound,
    LastSeenPlayer
}