using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceDirection
{
    Up,
    Down,
    Flat
}

public class GunShot : MonoBehaviour
{
    public GameObject bulletEffectPrefab;
    public AudioClip shotSound;
    [SerializeField]
    PlayerBodyController playerController;
    public BulletCountController bulletCountController;
    public Reload reload;

    [SerializeField]
    private PlayerWeapon playerWeapon;
    private FaceDirection faceDirection;


    void Start()
    {
        // 武器情報の初期化
        playerWeapon.Init();

        faceDirection = FaceDirection.Flat;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            // 左クリックが押されたら
            if (Input.GetButtonDown("Fire1"))
            {
                // 弾が生成できるかを確認し、生成するか、リロードする
                TryShoot();
            }
            // 向いている方向の取得
            faceDirection = GetFaceDirection();
        }
    }

    /// <summary>
    /// 向いている方向の取得
    /// </summary>
    /// <returns></returns>
    private FaceDirection GetFaceDirection()
    {
        // Wか↑を押した時
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            return FaceDirection.Up;
        }

        // Sか↓を押した時
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            return FaceDirection.Down;
        }

        return FaceDirection.Flat;
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
            reload.ReloadBullets(playerWeapon.CurrentWeaponLevelData.maxAmmo, playerWeapon.CurrentWeaponLevelData.reloadTime);
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
        Debug.Log("最初" + transform.eulerAngles);
        FaceDirection currentFaceDirection = faceDirection;

        // プレイヤーの向いている方向から角度を取得
        float offsetAngle = GetAngleFromFaceDirection();

        // 上か下を向いている場合
        if (currentFaceDirection != FaceDirection.Flat)
        {
            // 発射位置を頭上か足元に変更
            ChangePosition(playerController.playerLookDirection);

            // 回転を変更
            ChangeAngle(offsetAngle);
        }
        Debug.Log("生成直前"+ transform.eulerAngles);

        // 向きに関わらず、共通の処理
        // 高さの調整値を向きから取得
        float offsetY = GetOffsetFromFaceDirection();

        // 弾をインスタンス化する
        BulletController bullet = Instantiate(playerWeapon.CurrentWeaponLevelData.gunShellPrefab,
            new Vector3(transform.position.x, transform.position.y + offsetY + playerWeapon.CurrentWeaponLevelData.prefabPositionOffsetY, 0), transform.rotation);

        bullet.transform.eulerAngles = transform.eulerAngles;

        // マズルフラッシュをインスタンス化する
        GameObject bulletFire = Instantiate(bulletEffectPrefab,
            new Vector2(transform.position.x, transform.position.y + offsetY), transform.rotation);

        // 弾の設定を行い発射
        if (faceDirection == FaceDirection.Flat)
        {
            bullet.Shoot(
                new Vector2(playerWeapon.CurrentWeaponLevelData.shotSpeed * playerController.playerLookDirection, 0),
                playerWeapon.CurrentWeaponLevelData.maxDamage, playerWeapon.CurrentWeaponLevelData.minDamage);
        }
        else
        {
            if (currentFaceDirection == FaceDirection.Up)
            {
                bullet.Shoot(
                new Vector2(0, playerWeapon.CurrentWeaponLevelData.shotSpeed * 1),
                playerWeapon.CurrentWeaponLevelData.maxDamage, playerWeapon.CurrentWeaponLevelData.minDamage);
            }
            else
            {
                bullet.Shoot(
                new Vector2(0, playerWeapon.CurrentWeaponLevelData.shotSpeed * -1),
                playerWeapon.CurrentWeaponLevelData.maxDamage, playerWeapon.CurrentWeaponLevelData.minDamage);
            }
        }

        // 発射音を再生する
        AudioSource.PlayClipAtPoint(shotSound, transform.position);

        // 回転を修正
        ChangeAngle(0);

        Debug.Log("生成後" + transform.eulerAngles);

        // 上も下も向いていない場合
        if (currentFaceDirection == FaceDirection.Flat)
        {
            StartCoroutine(RespawnBulletEffect(playerWeapon.CurrentWeaponLevelData.shotRange, bullet.gameObject));
            // 終了
            return;
        }

        // ここは上か下を向いている時に動く
        StartCoroutine(RespawnBulletEffect(playerWeapon.CurrentWeaponLevelData.shotRange, bullet.gameObject, 0.6f));

        // 発射位置を元の位置に戻す
        ChangePosition(-playerController.playerLookDirection);
    }

    /// <summary>
    /// 回転を変更
    /// </summary>
    /// <param name="offsetAngle"></param>
    private void ChangeAngle(float offsetAngle)
    {
        transform.eulerAngles = new Vector3(0, 0, offsetAngle);
    }

    /// <summary>
    /// 位置を変更
    /// </summary>
    /// <param name="direction"></param>
    private void ChangePosition(float direction)
    {
        Vector2 pos = new Vector2(transform.position.x - (0.68f * direction), transform.position.y);
        transform.position = pos;
    }

    /// <summary>
    /// プレイヤーの向きから Y軸の調整値を取得
    /// </summary>
    /// <returns></returns>
    private float GetOffsetFromFaceDirection()
    {
        return faceDirection switch
        {
            FaceDirection.Up => 0.6f,
            FaceDirection.Down => -0.6f,
            FaceDirection.Flat => -0.2f,
            _ => 0f
        };
    }

    /// <summary>
    /// プレイヤーの向きから角度を取得
    /// </summary>
    /// <returns></returns>
    private float GetAngleFromFaceDirection()
    {
        return faceDirection switch
        {
            FaceDirection.Up => 90f,
            FaceDirection.Down => -90f,
            FaceDirection.Flat => 0f,
            _ => 0f
        };
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
            GameObject newBulletFire = Instantiate(bulletEffectPrefab,
                new Vector2(bullet.transform.position.x, bullet.transform.position.y + offset),
                    bullet.transform.rotation);

            Destroy(bullet);
        }
    }





    
}
