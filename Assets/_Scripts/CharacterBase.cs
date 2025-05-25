using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class CharacterBase : MonoBehaviour, ICharacter
{
    public Clothes clothes;
    public Character character;
    public Health hp;
    public event Action AnnounceShoved;
    public event Action AnnounceSlept;


    public void Awake()
    {
        if (hp != null)
        {
            hp.AnnounceHPChangedBy += React;
            hp.AnnounceHitByWeapon += ReactToWeapon;
        }
    }

    private void ReactToWeapon(WeaponSO obj)
    {
        if (obj.weaponDamage == 0)
        {
            AnnounceSlept?.Invoke();
            Debug.Log("hit by tranq!");
        }
    }

    private void React(int incomingDamage)
    {
        if (incomingDamage == 0)
        {
            AnnounceShoved?.Invoke();
        }
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
        if (hp != null)
        {
            hp.AnnounceHPChangedBy -= React;
            hp.AnnounceHitByWeapon -= ReactToWeapon;
        }
    }
}
