using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponSO : ScriptableObject
{
    public float range;
    public float fireRate;

    public int weaponDamage;
    
    public float weaponForce;
    public float weaponForceTorque;

    public Sprite weaponSprite;

    public SoundData soundData;
}
