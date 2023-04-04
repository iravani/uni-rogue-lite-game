
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    public PlayerMove playerMove;

    public Animator PlayerAnimator;
    public Animator DunAnimator;

    public float mouseSenceWhileAimMultiplier = 0.5f;
    public float playerSpeedWhileAimMultiplier = 0.5f;

    public static bool canReload = false;
    public bool isReloading = false;
    public static bool isAiming = false;

    public static float dunFireRateTime = 0.2f;
    float dunNextFireTime = 0;

    public CameraRecoil camRecoil;
    public WeaponRecoil weaponRecoil;

    public Gun Dun;

    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isAiming = true;
            PlayerMove.mouseSence = PlayerMove.defaultMouseSence * mouseSenceWhileAimMultiplier;
            PlayerMove.speedMultiplier = playerSpeedWhileAimMultiplier;

            image1.GetComponent<Animator>().Play("1in");
            image2.GetComponent<Animator>().Play("2in");
            image3.GetComponent<Animator>().Play("3in");
            image4.GetComponent<Animator>().Play("4in");

            PlayerMove.isRunning = false;
            PlayerMove.speed = playerMove.walkSpeed;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;
            PlayerMove.mouseSence = PlayerMove.defaultMouseSence;
            PlayerMove.speedMultiplier = 1;

            image1.GetComponent<Animator>().Play("1out");
            image2.GetComponent<Animator>().Play("2out");
            image3.GetComponent<Animator>().Play("3out");
            image4.GetComponent<Animator>().Play("4out");
        }

        if (isAiming)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                if (Dun.currentBulletCountOnGun > 0 && !isReloading)
                {
                    Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(Dun.allBulletsOnInventory > 0 && Dun.currentBulletCountOnGun < Dun.maxBulletCount)
            {
                Reload();
            }
        }

        PlayerAnimator.SetBool("Aim", isAiming);
        DunAnimator.SetBool("Aim", isAiming);
    }
    
    public void Shoot()
    {
        DunAnimator.SetTrigger("Shoot");
        PlayerAnimator.SetTrigger("Shoot");

        Dun.currentBulletCountOnGun--;

        camRecoil.Fire();
        weaponRecoil.Fire();
    } 

    public void Reload()
    {
        isAiming = false;
        isReloading = true;
        DunAnimator.SetBool("Reloading", isReloading);
        PlayerAnimator.SetBool("Reloading", isReloading);

        StartCoroutine(ApplyReloadTime());

        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }

    IEnumerator ApplyReloadTime()
    {
        yield return new WaitForSeconds(Dun.reloadTime);
        Dun.Reload();
        isReloading = false;
        DunAnimator.SetBool("Reloading", isReloading);
        PlayerAnimator.SetBool("Reloading", isReloading);
    }
}
