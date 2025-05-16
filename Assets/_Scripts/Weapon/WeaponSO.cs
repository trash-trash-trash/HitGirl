using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject, IWeapon
{
    public MonoBehaviour associatedLogic;
    
    public float range;
    public float fireRate;
    public float maxSound;
    public float minSound;
    public virtual void AggroAction()
    {
        
    }
}
