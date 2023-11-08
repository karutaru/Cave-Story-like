using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangeButton : MonoBehaviour
{
    public GunShot gunShot;
    public GunRotation gunRotation;

    public int weaponID;


    // ボタンが押されたときに呼ばれるメソッド
    public void OnButtonPressed()
    {
        gunShot.ChangeWeapon(weaponID);
        gunRotation.ChangeWeapon(weaponID);
    }
}
