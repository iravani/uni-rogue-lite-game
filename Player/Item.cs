using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { 
        multi,
        mono
    }

    public enum ItemItself { dunAmmo , shotgunAmmo}

    public string item_name;
    public ItemType itemType;
    public ItemItself itemItSelfType;
    public int itemCount;
}
