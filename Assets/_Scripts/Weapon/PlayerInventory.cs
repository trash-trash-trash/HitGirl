using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public PlayerInputHandler playerInputHandler;

    public WeaponSO selectedWeapon;
    public List<WeaponSO> weapons = new List<WeaponSO>();

    void Start()
    {
        playerInputHandler.AnnounceInventory01 += SelectInventory01;
        playerInputHandler.AnnounceInventory02 += SelectInventory02;
        playerInputHandler.AnnounceNextInventory += NextInventory;
        playerInputHandler.AnnounceAggressiveAction += TryAggroAction;
    }

    private void SelectInventory01(InputAction.CallbackContext obj)
    {
        selectedWeapon = SelectInventoryWithIntIndex(0);
    }

    private void SelectInventory02(InputAction.CallbackContext obj)
    {
        selectedWeapon = SelectInventoryWithIntIndex(1);
    }

    private void NextInventory(InputAction.CallbackContext obj)
    {
        //cycle thru list starting at 0 and going upwards. if reach maximum, cycle back to 0

        int currentIndex = weapons.IndexOf(selectedWeapon); 
        int nextIndex = (currentIndex + 1) % weapons.Count; 
        selectedWeapon = weapons[nextIndex];
    }

    private void TryAggroAction(InputAction.CallbackContext obj)
    {
        IWeapon selectWep = selectedWeapon;
        selectedWeapon.AggroAction();
    }

    WeaponSO SelectInventoryWithIntIndex(int index)
    {
        return selectedWeapon = weapons[index];
    }
    
    
}
