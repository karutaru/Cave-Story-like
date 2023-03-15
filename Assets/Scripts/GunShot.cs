using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    public float shotSpeed;
    public GameObject bulletPrefab;
    public GameObject bulletEffectPrefab;
    private GameObject bulletFire;
    //private GameObject bullet;
    public Transform firePoint;
    public Vector2 temp;
    public AudioClip shotSound;
    public Vector2 endPosition;
    [SerializeField]
    PlayerController playerController;

    void Start()
    {
        GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // プレイヤーの向いている方向を取得する
        Vector2 direction = new Vector2 (playerController.transform.localScale.x * -1, 0);

        // 弾をインスタンス化する
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject bulletFire = Instantiate(bulletEffectPrefab, new Vector2 (firePoint.position.x, firePoint.position.y - 0.2f), firePoint.rotation);
        temp = new Vector2 (firePoint.position.x, firePoint.position.y);

        // 弾に速度を与える
        bullet.GetComponent<Rigidbody2D>().velocity = direction * shotSpeed;

        // 発射音を再生する
        AudioSource.PlayClipAtPoint(shotSound, transform.position);

        StartCoroutine(RespawnBulletEffect(0.3f, bullet));
    }

    IEnumerator RespawnBulletEffect(float delay, GameObject bullet)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            GameObject newBulletFire = Instantiate(bulletEffectPrefab, new Vector2 (bullet.transform.position.x, bullet.transform.position.y - 0.2f), bullet.transform.rotation);
        }
    }
}
