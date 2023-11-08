using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangeButton : MonoBehaviour
{
    public GunShot gunShot;
    public GunRotation gunRotation;

    public int weaponID;


    // �{�^���������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnButtonPressed()
    {
        gunShot.ChangeWeapon(weaponID);
        gunRotation.ChangeWeapon(weaponID);
    }
}
