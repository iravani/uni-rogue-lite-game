using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject Dun;
    public GameObject Sun;

    public static int gun = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switchGun();
        }
    }

    void switchGun()
    {
        if (gun == 1)
            gun = 2;
        else
            gun = 1;

        if(gun == 1)
        {
            Sun.SetActive(false);
            gameObject.GetComponent<ShootingSun>().enabled = false;

            Dun.SetActive(true);
            gameObject.GetComponent<ShootingScript>().enabled = true;
        }
        else
        {
            Dun.SetActive(false);
            gameObject.GetComponent<ShootingScript>().enabled = false;

            Sun.SetActive(true);
            gameObject.GetComponent<ShootingSun>().enabled = true;
        }
    }
}
