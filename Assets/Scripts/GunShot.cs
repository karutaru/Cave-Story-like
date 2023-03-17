using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    private bool UpNow; //上を向いているか
    private Vector2 bulletLR; //マズルフラッシュ用の弾の向き
    public float shotSpeed;
    public GameObject bulletPrefab;
    public GameObject bulletEffectPrefab;
    private GameObject bulletFire;
    public Transform firePoint;
    public AudioClip shotSound;
    public Vector2 endPosition;
    [SerializeField]
    PlayerController playerController;
    public BulletCountController bulletCountController;
    public bool canShot; //弾があるか


    void Start()
    {
        GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) //左クリックが押されたら
        {
            if (canShot == false) //弾がないなら
            {
                bulletCountController.ShotNow();
                
            } else { //弾があるなら
                //Shootを呼び出す
                Shoot();
                bulletCountController.ShotNow();
            }
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Wか↑を押した時
        {
            UpNow = true;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Wか↑を離した時
        {
             UpNow = false;
        }
    }

    void Shoot()
    {
        if (UpNow == true) //Wか↑を押した時
        {
            // プレイヤーの向いている方向を取得する
            Vector2 direction = new Vector2 (playerController.playerLookDirection, 0);

            //位置を変更
            Transform myTransform = this.transform;
            Vector2 pos = new Vector2 (myTransform.position.x -(0.68f * direction.x), myTransform.position.y);
            myTransform.position = pos;

            //回転を変更
            Vector3 localAngle = myTransform.localEulerAngles;
            Vector3 pos2 = new Vector3 (0, 0, localAngle.z + -90f);
            myTransform.localEulerAngles = pos2;

            // 弾をインスタンス化する
            GameObject bullet = Instantiate(bulletPrefab, new Vector3 (firePoint.position.x, firePoint.position.y + 0.6f, 0), firePoint.rotation);
            // マズルフラッシュをインスタンス化する
            GameObject bulletFire = Instantiate(bulletEffectPrefab, new Vector2 (firePoint.position.x, firePoint.position.y + 0.6f), firePoint.rotation);

            if (bullet != null)
            {
                bulletLR = direction;
            }

            // 弾に速度を与える
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, shotSpeed);

            // 発射音を再生する
            AudioSource.PlayClipAtPoint(shotSound, transform.position);

            StartCoroutine(RespawnUpBulletEffect(0.3f, bullet));

            //回転を修正
            myTransform = this.transform;
            localAngle = myTransform.localEulerAngles;
            pos2 = new Vector3 (0, 0, localAngle.z + 90f);
            myTransform.localEulerAngles = pos2;

            //位置を修正
            myTransform = this.transform;
            pos = new Vector2 (myTransform.position.x + (0.68f * direction.x), myTransform.position.y);
            myTransform.position = pos;
        }



        if (UpNow == false)
        {
            // プレイヤーの向いている方向を取得する
            Vector2 direction = new Vector2 (playerController.playerLookDirection, 0);
            // 弾をインスタンス化する
            GameObject bullet = Instantiate(bulletPrefab, new Vector2 (firePoint.position.x, firePoint.position.y - 0.2f), firePoint.rotation);
            // マズルフラッシュをインスタンス化する
            GameObject bulletFire = Instantiate(bulletEffectPrefab, new Vector2 (firePoint.position.x, firePoint.position.y - 0.2f), firePoint.rotation);

            //temp = new Vector2 (firePoint.position.x, firePoint.position.y);

            // 弾に速度を与える
            bullet.GetComponent<Rigidbody2D>().velocity = direction * shotSpeed;

            // 発射音を再生する
            AudioSource.PlayClipAtPoint(shotSound, transform.position);

            StartCoroutine(RespawnBulletEffect(0.3f, bullet));
        }
    }



    IEnumerator RespawnBulletEffect(float delay, GameObject bullet)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            GameObject newBulletFire = Instantiate(bulletEffectPrefab, new Vector2 (bullet.transform.position.x, bullet.transform.position.y - 0.2f), bullet.transform.rotation);
        }
    }

    IEnumerator RespawnUpBulletEffect(float delay, GameObject bullet)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            //Vector2 b = new Vector2 (bullet.transform.position.x, bullet.transform.position.y);
            GameObject newBulletFire = Instantiate(bulletEffectPrefab, new Vector2 (bullet.transform.position.x, bullet.transform.position.y + 0.6f), bullet.transform.rotation);
        }
    }

    public void BulletEmpty(bool amount)
    {
        canShot = amount;
    }
}
