using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCountController : MonoBehaviour
{
    [SerializeField]
    private int currentAmmoCount = 0; // 現在の弾数
    public int CurrentAmmoCount => currentAmmoCount; // プロパティ（外部アクセス用）

    private int gunMaxAmmo; // 最大装弾数

    [SerializeField]
    private BulletDisplay bulletDisplay;
	

    void Start()
    {
        currentAmmoCount = gunMaxAmmo;
    }

    public void StartBullets(int amount)
    {
        gunMaxAmmo = amount;
        bulletDisplay.UpdateBulletDisplay(gunMaxAmmo);
    }

    /// <summary>
    /// 残弾数を更新（減算）
    /// </summary>
    public void CountDownBullet()
    {
        // 現在の弾数から1発分を引く
        currentAmmoCount--;
        bulletDisplay.UpdateBulletDisplay(currentAmmoCount);
    }

    /// <summary>
    /// 残弾数のリロード
    /// </summary>
    public void Reloaded(int amount)
    {
        // 最大値に戻す
        currentAmmoCount = amount;
        bulletDisplay.UpdateBulletDisplay(currentAmmoCount);
    }
}
