using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun : MonoBehaviour
{
    public enum BulletType { 
        Dun_Ammo,
        Shotgun_Ammo
    }

    public Inventory inventory;

    public string gunName;
    public BulletType bulletType;
    public int maxBulletCount;
    public int currentBulletCountOnGun;
    public int allBulletsOnInventory;
    public float fireTime;
    public float reloadTime;

    private int AmmoIndex = 1000;

    private void Start()
    {
        if(bulletType == BulletType.Dun_Ammo)
        {
            for (int i = 0; i < inventory.items.Length; i++)
            {
                if (bulletType == BulletType.Dun_Ammo)
                {
                    if (inventory.items[i].itemItSelfType == Item.ItemItself.dunAmmo)
                    {
                        allBulletsOnInventory = inventory.items[i].itemCount;
                        AmmoIndex = i;
                        break;
                    }
                }
                //else
                //{
                //    if (inventory.items[i].itemItSelfType == Item.ItemItself.shotgunAmmo)
                //    {
                //        allBulletsOnInventory = inventory.items[i].itemCount;
                //        AmmoIndex = i;
                //        break;
                //    }
                //}
            }

            if (AmmoIndex != 1000)
            {
                if (allBulletsOnInventory > maxBulletCount)
                {
                    currentBulletCountOnGun = maxBulletCount;
                    allBulletsOnInventory -= maxBulletCount;
                    inventory.items[AmmoIndex].itemCount -= maxBulletCount;
                }
                else
                {
                    currentBulletCountOnGun = allBulletsOnInventory;
                    allBulletsOnInventory = 0;
                    inventory.items[AmmoIndex].itemCount = 0;
                }

            }
        }
        else
        {
            for (int i = 0; i < inventory.items.Length; i++)
            {
                if (bulletType == BulletType.Shotgun_Ammo)
                {
                    if (inventory.items[i].itemItSelfType == Item.ItemItself.shotgunAmmo)
                    {
                        allBulletsOnInventory = inventory.items[i].itemCount;
                        AmmoIndex = i;
                        break;
                    }
                }
                //else
                //{
                //    if (inventory.items[i].itemItSelfType == Item.ItemItself.dunAmmo)
                //    {
                //        allBulletsOnInventory = inventory.items[i].itemCount;
                //        AmmoIndex = i;
                //        break;
                //    }
                //}
            }

            if (AmmoIndex != 1000)
            {
                if (allBulletsOnInventory > maxBulletCount)
                {
                    currentBulletCountOnGun = maxBulletCount;
                    allBulletsOnInventory -= maxBulletCount;
                    inventory.items[AmmoIndex].itemCount -= maxBulletCount;
                }
                else
                {
                    currentBulletCountOnGun = allBulletsOnInventory;
                    allBulletsOnInventory = 0;
                    inventory.items[AmmoIndex].itemCount = 0;
                }

            }
        }

        


    }

    private void Update()
    {
        
    }

    public void Reload()
    {
        if(bulletType == BulletType.Dun_Ammo)
        {
            if (maxBulletCount - currentBulletCountOnGun >= allBulletsOnInventory)
            {
                currentBulletCountOnGun += allBulletsOnInventory;
                allBulletsOnInventory = 0;
                inventory.items[AmmoIndex].itemCount = 0;
            }
            else
            {
                int cb = currentBulletCountOnGun;
                currentBulletCountOnGun = maxBulletCount;
                allBulletsOnInventory -= (maxBulletCount - cb);
                inventory.items[AmmoIndex].itemCount = allBulletsOnInventory;
            }
        }
        else
        {
            if(allBulletsOnInventory > 0)
            {
                currentBulletCountOnGun++;
                allBulletsOnInventory--;
                inventory.items[AmmoIndex].itemCount--;
                ShootingSun.reloadTimes--;
            }
            else
            {
                ShootingSun.canReload = false;
                ShootingSun.reloadTimes = 0;
            }
        }
        
    }
}
