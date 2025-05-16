using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class CharacterBase : MonoBehaviour, ICharacter, IHear
{
    public Character character;
    public Health hp;
    public SoundData mostRecentSoundHeard;
    
    private bool canHear = true;

    public bool CanHear
    {
        get { return canHear; }
        set { canHear = value; }
    }

    public event Action AnnounceShoved;
    public event Action<SoundData> AnnounceSoundHeard;

    public void Awake()
    {
        hp.AnnounceHPChangedBy += React;
    }

    private void React(int incomingDamage)
    {
        if (incomingDamage == 0)
        {
            AnnounceShoved?.Invoke();
        }
    }

    public void HeardSound(SoundData sound)
    {
        if (!canHear)
            return;
        
        mostRecentSoundHeard = sound;
        AnnounceSoundHeard?.Invoke(sound);
    }

    public void FlipCanHear(bool input)
    {
        CanHear = input;
    }

    public Character ReturnCharacter()
    {
        return character; 
    }

    public CharacterBase ReturnCharacterBase()
    {
        return this;
    }

    void OnDisable()
    {
        hp.AnnounceHP -= React;
    }
}
