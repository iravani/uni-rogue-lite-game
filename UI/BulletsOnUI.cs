using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsOnUI : MonoBehaviour
{
    public Text currentBullets;
    public Text inventoryBullets;

    public Gun Dun;

    private void LateUpdate()
    {
        currentBullets.text = Dun.currentBulletCountOnGun.ToString();
        inventoryBullets.text = Dun.allBulletsOnInventory.ToString();
    }
}
