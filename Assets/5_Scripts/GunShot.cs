using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//public enum FaceDirection
//{
//    Up,
//    Down,
//    Flat
//}

public class GunShot : MonoBehaviour
{
    // スクリプタブルオブジェクトの登録（アセットをアサイン）
    [SerializeField, Header("武器レベルデータのスクリプタブルオブジェクト")]
    private WeaponLevelDataSO weaponLevelDataSO;

    public GameObject bulletEffectPrefab;
    public AudioClip shotSound;
    [SerializeField]
    PlayerBodyController playerController;
    public BulletCountController bulletCountController;
    public Reload reload;

    private WeaponLevelData currentWeaponLevelData; // 現在の武器レベルデータ（弾の速度、ダメージ、プレハブ）
    public WeaponLevelData CurrentWeaponLevelData => currentWeaponLevelData; // プロパティ

    [SerializeField]
    private PlayerWeapon playerWeapon;
    //private FaceDirection faceDirection;
    public float shotTimeIntervalCount = 0; // 射撃間隔のカウンターを追加
    public bool isShot = true;              // 射撃可能な状態か？
    private bool canShoot = true;           // 射撃可能かどうかのフラグを追加
    private int currentWeaponID = 0;        // 現在持っている武器のID
    private float firstAccuracy = 0;

    [SerializeField]
    private GunRotation gunRotation;


    void Start()
    {
        bulletCountController.StartBullets(weaponLevelDataSO.weaponLevelDataList[currentWeaponID].maxAmmo);
}

    private void Update()
    {
        if (Time.timeScale == 1 && isShot)
        {
            // 左クリックが押されたかどうかをチェック
            if (Input.GetButtonDown("Fire1"))
            {
                // shotTimeIntervalCountがshotIntervalを超えていれば、またはcanShootがtrueであれば射撃
                if (shotTimeIntervalCount >= weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotInterval || canShoot)
                {
                    TryShoot();
                    shotTimeIntervalCount = 0; // 射撃後はカウンターをリセット
                    canShoot = false; // 射撃可能フラグをfalseに設定
                }
            }
            else if (Input.GetButton("Fire1"))
            {
                // 左クリックを押し続けている場合はカウンターを増やす
                if (!canShoot) // canShootがfalseのときのみカウンターを増やす
                {
                    shotTimeIntervalCount += Time.deltaTime * 100; // 実際の時間に基づいてカウンターを増やす
                    if (shotTimeIntervalCount >= weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotInterval)
                    {
                        // 射撃間隔が十分に経過していれば射撃
                        TryShoot();
                        shotTimeIntervalCount = 0; // 射撃後はカウンターをリセット
                    }
                }
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                // 左クリックを離したら、canShootをfalseに設定し、カウンターを増加させ続ける
                canShoot = false;
            }
            else if (!Input.GetButton("Fire1") && !canShoot) // 左クリックが押されていない間はカウンターを増やす
            {
                shotTimeIntervalCount += Time.deltaTime * 100; // 実際の時間に基づいてカウンターを増やす
                if (shotTimeIntervalCount >= weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotInterval)
                {
                    // 射撃間隔が十分に経過していれば、次にクリックしたときにすぐに射撃できるようにする
                    canShoot = true;
                }
            }
        }
    }

    public void ChangeWeapon(int amount)
    {
        currentWeaponID = amount;
    }

    /// <summary>
    /// 向いている方向の取得
    /// </summary>
    /// <returns></returns>
    //private FaceDirection GetFaceDirection()
    //{
    //    // Wか↑を押した時
    //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    //    {
    //        return FaceDirection.Up;
    //    }

    //    // Sか↓を押した時
    //    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
    //    {
    //        return FaceDirection.Down;
    //    }

    //    return FaceDirection.Flat;
    //}

    /// <summary>
    /// 弾が生成できるか確認してから弾を生成する
    /// もしも生成できない場合にはリロードする
    /// </summary>
    private void TryShoot()
    {
        // 弾が無いなら
        if (bulletCountController.CurrentAmmoCount <= 0)
        {
            // リロード
            reload.ReloadBullets(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].maxAmmo, DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].reloadTime);
        }
        else
        {
            // 弾があるなら生成
            GenerateBullet();

            // 残弾数更新
            bulletCountController.CountDownBullet();
        }
    }


    /// <summary>
    /// 弾の生成
    /// </summary>
    private void GenerateBullet()
    {
        if (playerWeapon == null)
        {
            Debug.LogError("playerWeapon is null!");
            return;
        }

        if (DataBase.instance.weaponSO == null)
        {
            Debug.LogError("CurrentWeaponLevelDataSO is null!");
            return;
        }

        if (DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].gunShellPrefab == null)
        {
            Debug.LogError("gunShellPrefab is null!");
            return;
        }
        // 弾の位置の初期ランダム性
        firstAccuracy = Random.Range(- DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].firstAccuracy, weaponLevelDataSO.weaponLevelDataList[currentWeaponID].firstAccuracy);

        // 弾をインスタンス化する
        BulletController bullet = Instantiate(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].gunShellPrefab,
            new Vector3(transform.position.x, transform.position.y + DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].prefabPositionOffsetY + firstAccuracy, 0), transform.rotation);

        bullet.transform.localScale = new Vector3(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shellSize, DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shellSize, 1f);

        //bullet.transform.eulerAngles = transform.eulerAngles;

        // マズルフラッシュをインスタンス化する
        GameObject bulletFire = Instantiate(bulletEffectPrefab,
            new Vector2(transform.position.x, transform.position.y), transform.rotation);

        bullet.transform.eulerAngles = gunRotation.BulletEulerAngles;

        // 弾の設定を行い発射
        bullet.Shoot(
            gunRotation.BulletDirection * DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shotSpeed,
            DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].maxDamage, DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].minDamage);

        // 発射音を再生する
        AudioSource.PlayClipAtPoint(shotSound, transform.position);

        // 回転を修正
        //ChangeAngle(0);

        //Debug.Log("生成後" + transform.eulerAngles);

        //// 上も下も向いていない場合
        //if (currentFaceDirection == FaceDirection.Flat)
        //{
        //    StartCoroutine(RespawnBulletEffect(playerWeapon.CurrentWeaponLevelData.shotRange, bullet.gameObject));
        //    // 終了
        //    return;
        //}

        // ここは上か下を向いている時に動く
        StartCoroutine(RespawnBulletEffect(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shotRange, bullet.gameObject, 0f));

        // 発射位置を元の位置に戻す
        //ChangePosition(-playerController.playerLookDirection);
    }

    /// <summary>
    /// 回転を変更
    /// </summary>
    /// <param name="offsetAngle"></param>
    //private void ChangeAngle(float offsetAngle)
    //{
    //    transform.eulerAngles = new Vector3(0, 0, offsetAngle);
    //}

    /// <summary>
    /// 位置を変更
    /// </summary>
    /// <param name="direction"></param>
    //private void ChangePosition(float direction)
    //{
    //    Vector2 pos = new Vector2(transform.position.x - (0.68f * direction), transform.position.y);
    //    transform.position = pos;
    //}

    ///// <summary>
    ///// プレイヤーの向きから Y軸の調整値を取得
    ///// </summary>
    ///// <returns></returns>
    //private float GetOffsetFromFaceDirection()
    //{
    //    return faceDirection switch
    //    {
    //        FaceDirection.Up => 0.6f,
    //        FaceDirection.Down => -0.6f,
    //        FaceDirection.Flat => -0.2f,
    //        _ => 0f
    //    };
    //}

    ///// <summary>
    ///// プレイヤーの向きから角度を取得
    ///// </summary>
    ///// <returns></returns>
    //private float GetAngleFromFaceDirection()
    //{
    //    return faceDirection switch
    //    {
    //        FaceDirection.Up => 90f,
    //        FaceDirection.Down => -90f,
    //        FaceDirection.Flat => 0f,
    //        _ => 0f
    //    };
    //}

    /// <summary>
    /// 終端のマズルフラッシュの生成
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="bullet"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    private IEnumerator RespawnBulletEffect(float delay, GameObject bullet, float offset = 0f)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            GameObject newBulletFire = Instantiate(bulletEffectPrefab,
                new Vector2(bullet.transform.position.x, bullet.transform.position.y + offset),
                    bullet.transform.rotation);

            Destroy(bullet);
        }
    }





    
}
