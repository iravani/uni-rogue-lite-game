using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public Vector3 CameraRecoilRotation = new Vector3(45, 45, 45);

    public Transform recoilPosition;
    public Transform recoilRotationPoint;

    public float positionalRecoilSpeed = 6;
    public float rotationalRecoilSpeed = 6;

    public float positionalReturnSpeed = 25;
    public float rotationalReturnSpeed = 25;

    public Vector3 recoilRotation = new Vector3(10, 5, 7);
    public Vector3 recoilKickBack = new Vector3(0.025f, 0, -0.2f);

    Vector3 rotationalRecoil;
    Vector3 positionalRecoil;
    Vector3 rot;

    private void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        rot = Vector3.Slerp(rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        recoilRotationPoint.localRotation = Quaternion.Euler(rot);
    }

    public void Fire()
    {
        rotationalRecoil = new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
        positionalRecoil = new Vector3(Random.Range(-recoilKickBack.x , recoilKickBack.x),Random.Range(-recoilKickBack.y, recoilKickBack.y),recoilKickBack.z);
    }
}
