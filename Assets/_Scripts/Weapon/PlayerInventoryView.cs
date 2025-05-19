using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryView : MonoBehaviour
{
   public PlayerInventory inventory;

   public Image weaponIconImg;

   void Start()
   {
      inventory.AnnounceWeaponEquipped += ChangeIcon;
   }

   private void ChangeIcon(WeaponSO newWep)
   {
      weaponIconImg.sprite = newWep.weaponSprite;
   }
}
