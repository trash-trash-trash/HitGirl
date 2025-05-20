// using System;
// using UnityEngine;
//
// public class Health : MonoBehaviour
// {
//     private int currentHP;
//
//     public int CurrentHP
//     {
//         get { return currentHP; }
//         set { currentHP = value; }
//     }
//
//     private int maxHP;
//
//     public int MaxHP
//     {
//         get { return maxHP; }
//         set { maxHP = value; }
//     }
//
//     private bool alive;
//     
//     public bool Alive { get; private set; } = true;
//
//     public event Action<int> AnnounceHP;
//
//     public void ChangeHP(int value)
//     {
//         if (!alive)
//             return;
//         
//         int hp = currentHP += value;
//
//         //die
//         if (hp <= 0)
//         {
//             alive = false;
//             hp = 0;
//         }
//         
//         else if (hp > 0)
//         {
//             if (hp > maxHP)
//                 hp = maxHP;
//
//             alive = true;
//         }
//         CurrentHP = hp;
//         AnnounceHP?.Invoke(currentHP);
//     }
//
//     public void ChangeMaxHP(int value)
//     {
//         int newMaxHp = maxHP += value;
//         
//     }
// }

using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    
    //condense...
    public event Action<int> AnnounceHP;
    public event Action<int> AnnounceHPChangedBy;
    public event Action<WeaponSO> AnnounceHitByWeapon;
    public event Action<HealthStatus> AnnounceHealthStatus;
    
    [SerializeField]
    private int currentHP;
    
    [SerializeField]
    private int maxHP = 1;

    [SerializeField]
    private bool alive = false;

    public bool Alive
    {
        get => alive;
        set => alive = value;
    }

    [SerializeField] 
    private HealthStatus healthStatus;

    public HealthStatus _healthStatus
    {
        get { return healthStatus; }
        set
        {
            healthStatus = value;
            AnnounceHealthStatus?.Invoke(healthStatus);
        }
    }
    public bool CanChangeHP { get; private set; } = true;

    public int CurrentHP
    {
        get { return currentHP; }
        private set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);
            Alive = currentHP > 0;
            CanChangeHP &= Alive;
            if (!Alive)
                _healthStatus = HealthStatus.Dead;
            
            AnnounceHP?.Invoke(currentHP);
        }
    }


    public virtual void ChangeHP(int value)
    {
        if (!CanChangeHP)
            return;

        CurrentHP += value;
        AnnounceHPChangedBy?.Invoke(value);
    }

    public virtual void HitByWeapon(WeaponSO weaponSo)
    {
        AnnounceHitByWeapon?.Invoke(weaponSo);

        if (weaponSo.weaponDamage == 0)
            _healthStatus = HealthStatus.Asleep;
        
        ChangeHP(weaponSo.weaponDamage);
    }
}

[Serializable]
public enum HealthStatus
{
    Fine,
    Asleep,
    Dead
}
