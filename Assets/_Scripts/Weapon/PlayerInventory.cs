using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public PlayerInputHandler playerInputHandler;

    public List<WeaponSlot> weaponSlots = new List<WeaponSlot>();
    public int selectedIndex = 0;
    
    [SerializeField, ReadOnly]
    private WeaponSO debug_SelectedWeapon;
    public WeaponSO SelectedWeapon => weaponSlots[selectedIndex].weaponSO;
    public IWeapon SelectedIWeapon => weaponSlots[selectedIndex].GetWeapon();

    private bool canSwitch = true;

    public event Action<WeaponSO> AnnounceWeaponEquipped;

    public bool CanSwitch
    {
        get { return canSwitch; }
        set { CanSwitch = value; }
    }
    
    private bool aiming = true;

    private bool canAim = true;

    void Start()
    {
        playerInputHandler.AnnounceInventory01 += SelectInventory01;
        playerInputHandler.AnnounceInventory02 += SelectInventory02;
        playerInputHandler.AnnounceNextInventory += NextInventory;
        playerInputHandler.AnnounceAggressiveAction += TryAggroAction;
        playerInputHandler.AnnounceUseAction += TryHolster;

        SelectInventory(0);
    }

    private void TryHolster(InputAction.CallbackContext obj)
    {
        if (aiming)
        {
            SelectedIWeapon.Holster();
            aiming = false;
            canSwitch = true;
        }
    }

    void SelectInventory(int index)
    {
        if (!canSwitch)
            return;
        if (index >= 0 && index < weaponSlots.Count) 
        { 
            selectedIndex = index;
            debug_SelectedWeapon = SelectedWeapon;
            SelectedIWeapon.Equip(SelectedWeapon);
            AnnounceWeaponEquipped?.Invoke(SelectedWeapon);
        }
    }
        
    void SelectInventory01(InputAction.CallbackContext context)
    {
        if (context.performed)
            SelectInventory(0);
    }

    void SelectInventory02(InputAction.CallbackContext context)
    {
        if (context.performed)
            SelectInventory(1);
    }
    

    void NextInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int nextIndex = (selectedIndex + 1) % weaponSlots.Count;
            SelectInventory(nextIndex);
        }
    }

    void TryAggroAction(InputAction.CallbackContext context)
    {
        if (!canAim)
            return;
        
        if (context.performed)
        {
            canSwitch = false;
            aiming = true;
            SelectedIWeapon?.Aim();
        }

        if (context.canceled && aiming)
        {
            canSwitch = false;
            canAim = false;
            aiming = false;
            SelectedIWeapon?.AggroAction();
            StartCoroutine(WaitForWeaponCooldown(SelectedWeapon.fireRate));
        }
    }

    IEnumerator WaitForWeaponCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canAim = true;
        canSwitch = true;
    }
}

[System.Serializable]
public class WeaponSlot
{
    public WeaponSO weaponSO;
    public MonoBehaviour weaponBehaviour;

    public IWeapon GetWeapon() => weaponBehaviour as IWeapon;
}
