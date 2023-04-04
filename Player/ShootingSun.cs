using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingSun : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    public PlayerMove playerMove;

    public Animator PlayerAnimator;
    public Animator SunAnimator;

    public float mouseSenceWhileAimMultiplier = 0.5f;
    public float playerSpeedWhileAimMultiplier = 0.5f;

    public static bool canReload = false;
    public bool isReloading = false;
    public static bool isAiming = false;

    public static float sunFireRateTime = 0.2f;
    float sunNextFireTime = 0;

    public CameraRecoil camRecoil;
    public WeaponRecoil weaponRecoil;

    public Gun Sun;

    public Inventory inventory;

    public static int reloadTimes;

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
                if (Sun.currentBulletCountOnGun > 0 && !isReloading)
                {
                    Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Sun.allBulletsOnInventory > 0 && Sun.currentBulletCountOnGun < Sun.maxBulletCount)
            {
                Reload();
            }
        }

        PlayerAnimator.SetBool("Aim", isAiming);
        SunAnimator.SetBool("Aim", isAiming);
        SunAnimator.SetInteger("ReloadTimes", reloadTimes);
        PlayerAnimator.SetInteger("ReloadTimes", reloadTimes);

    }

    public void Shoot()
    {
        SunAnimator.SetTrigger("Shoot");
        PlayerAnimator.SetTrigger("Shoot");

        Sun.currentBulletCountOnGun--;

        reloadTimes = Sun.maxBulletCount - Sun.currentBulletCountOnGun;

        
        camRecoil.Fire();
        weaponRecoil.Fire();
    }

    public void Reload()
    {
        isAiming = false;
        isReloading = true;
        SunAnimator.SetBool("Reloading", isReloading);
        PlayerAnimator.SetBool("Reloading", isReloading);

        for (int i = reloadTimes; i != 0; i--)
        {
            StartCoroutine(ApplyReloadTime());
            Debug.Log("R");
        }

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
        SunAnimator.SetTrigger("Load");
        PlayerAnimator.SetTrigger("Load");
        yield return new WaitForSeconds(Sun.reloadTime * reloadTimes * 2 + Sun.reloadTime);
        
        Sun.Reload();
        SunAnimator.SetTrigger("Load");
        PlayerAnimator.SetTrigger("Load");
        PlayerAnimator.SetBool("Reloading", isReloading);
        SunAnimator.SetBool("Reloading", isReloading);

        if (reloadTimes == 0)
            isReloading = false;

        SunAnimator.SetBool("Reloading", isReloading);
        PlayerAnimator.SetBool("Reloading", isReloading);
    }

}
