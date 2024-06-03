using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private int currentWeaponID = 1;        // 現在持っている武器のID
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
        firstAccuracy = Random.Range(-DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].firstAccuracy, weaponLevelDataSO.weaponLevelDataList[currentWeaponID].firstAccuracy);

        // 弾をインスタンス化する
        BulletController bullet = Instantiate(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].gunShellPrefab,
            new Vector3(transform.position.x, transform.position.y + DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].prefabPositionOffsetY + firstAccuracy, 0), transform.rotation);

        bullet.transform.localScale = new Vector3(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shellSize, DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shellSize, 1f);

        //// マズルフラッシュをインスタンス化する
        //GameObject bulletFire = Instantiate(bulletEffectPrefab,
        //    new Vector2(transform.position.x - 1f, transform.position.y), transform.rotation);

        // マズルフラッシュをインスタンス化する
        GameObject bulletFire = Instantiate(bulletEffectPrefab,
            gunRotation.transform.position + gunRotation.BulletDirection * 1.1f, // 銃の方向に1.1単位進んだ位置にマズルフラッシュを生成
            Quaternion.Euler(gunRotation.BulletEulerAngles)); // 銃の回転を適用

        bullet.transform.eulerAngles = gunRotation.BulletEulerAngles;

        // 弾の設定を行い発射
        bullet.Shoot(
            gunRotation.BulletDirection * DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shotSpeed,
            DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].maxDamage, DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].minDamage);

        // 発射音を再生する
        AudioSource.PlayClipAtPoint(shotSound, transform.position);

        // ここは上か下を向いている時に動く
        StartCoroutine(RespawnBulletEffect(DataBase.instance.weaponSO.weaponLevelDataList[currentWeaponID].shotRange, bullet.gameObject, 0f));
    }

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
            // bulletの位置に新しいマズルフラッシュエフェクトを生成
            GameObject newBulletFire = Instantiate(bulletEffectPrefab,
                new Vector2(bullet.transform.position.x, bullet.transform.position.y + offset),
                    bullet.transform.rotation);

            //// bulletにアタッチされているEffectScriptのOnDestroyメソッドを呼び出す
            //EffectScript effectScript = bullet.GetComponent<EffectScript>();
            //if (effectScript != null)
            //{
            //    effectScript.OnDestroy();
            //}

            Destroy(bullet);
        }
    }
}