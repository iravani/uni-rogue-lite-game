using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil: MonoBehaviour
{
    public WeaponRecoil DunRecoil;
    public WeaponRecoil SunRecoil;

    private WeaponRecoil currentWeapon;

    public float rotationSpeed = 6;
    public float returnSpeed = 25;

    public float rotationSpeedSun = 10;
    public float returnSpeedSun = 25;

    private Vector3 currentRotation;
    private Vector3 rot;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if(PlayerGun.gun == 1)
        {
            currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(rot);
        }
        else
        {
            currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeedSun * Time.deltaTime);
            rot = Vector3.Slerp(rot, currentRotation, rotationSpeedSun * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(rot);
        }
    }

    public void Fire()
    {
        if(PlayerGun.gun == 1)
        {
            currentRotation += new Vector3(-DunRecoil.CameraRecoilRotation.x, Random.Range(-DunRecoil.CameraRecoilRotation.y, DunRecoil.CameraRecoilRotation.y), Random.Range(-DunRecoil.CameraRecoilRotation.z, DunRecoil.CameraRecoilRotation.z));
        }
        else
        {
            currentRotation += new Vector3(-SunRecoil.CameraRecoilRotation.x, Random.Range(-SunRecoil.CameraRecoilRotation.y, SunRecoil.CameraRecoilRotation.y), Random.Range(-SunRecoil.CameraRecoilRotation.z, SunRecoil.CameraRecoilRotation.z));
        }
    }

}
