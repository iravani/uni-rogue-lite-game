using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil2 : MonoBehaviour
{
    public WeaponRecoil SunRecoil;

    private WeaponRecoil currentWeapon;

    public float rotationSpeed = 6;
    public float returnSpeed = 25;

    private Vector3 currentRotation;
    private Vector3 rot;

    private void Start()
    {
        currentWeapon = SunRecoil;
    }

    private void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }

    public void Fire()
    {
        currentRotation += new Vector3(-currentWeapon.CameraRecoilRotation.x, Random.Range(-currentWeapon.CameraRecoilRotation.y, currentWeapon.CameraRecoilRotation.y), Random.Range(-currentWeapon.CameraRecoilRotation.z, currentWeapon.CameraRecoilRotation.z));
        Debug.Log("HII");
    }

}
