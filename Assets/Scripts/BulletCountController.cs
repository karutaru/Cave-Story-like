using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCountController : MonoBehaviour
{
    private bool shotReady;                         //撃てるか否か
    public int currentAmmoCount;                  //現在の弾数
    public GameObject bulletUIPrefab;               //弾のUIプレハブ
	public GameObject bulletCountUI;                //親オブジェクト
    GameObject Obj;
    public GunShot gunShot;
    public Reload reload;
    public GameObject[] bullets; //弾数の配列
    public int gunMaxAmmo = 5; //最大装弾数
	

    void Start()
    {
        // 配列の初期化(銃の最大弾数だけ配列の要素数を用意する)
        bullets = new GameObject[gunMaxAmmo];
        currentAmmoCount = gunMaxAmmo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { // Rボタンを押したら

            // リロード
            reload.ReloadBullets(gunMaxAmmo); //弾の最大数を送る
            
        }
    }



	public void ShotNow(){
		Obj = (GameObject)Instantiate (bulletUIPrefab, new Vector2 (-1224f, 576f), Quaternion.identity);
		Obj.transform.parent = bulletCountUI.transform;


        currentAmmoCount -= 1; //現在の弾数から1発分を引く

        if (currentAmmoCount > 0) //弾数が0より多いなら
        {
            shotReady = true; //弾が撃てる
            gunShot.BulletEmpty(shotReady);
        } else {
            shotReady = false; //弾を撃てない
            gunShot.BulletEmpty(shotReady);
            // リロード
            reload.ReloadBullets(gunMaxAmmo); //弾の最大数を送る
        }
	}

    public void Reloaded(int amount)
    {
        currentAmmoCount = amount;
        shotReady = true; //弾が撃てる
        gunShot.BulletEmpty(shotReady);
    }
}
