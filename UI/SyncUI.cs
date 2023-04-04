using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncUI : MonoBehaviour
{
    public Image stamina;

    private void Update()
    {
        stamina.transform.localScale = new Vector3(PlayerStats.stamina / 100, 1, 1);
    }
}
