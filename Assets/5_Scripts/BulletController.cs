using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [LabelText("壁に当たると消える"), HideLabel]
    public bool isWallHit = true;

    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // プロパティ

    private void Start()
    {
        if (!isWallHit)
        {
            // 2つ目のCircle Collider 2DをONにする
            CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
            if (colliders.Length > 1)
            {
                colliders[1].enabled = true;
            }
        }
    }

    public void Shoot(Vector2 direction, int maxDamage, int minDamage) // --------------------------------------------------------------------------------------------------------------------------------------------
    {
        // 最大値と最小値からダメージを計算し、ダメージセット
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // 発射
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other) // トリガーに何かが触れたら ----------------------------------------------------------------------------------------------------------------------------------------
    {
        if (other.CompareTag("Obstacle") && isWallHit) // 障害物だったら
        {
            // 壁エフェクトを実体化する
            GameObject wallEffect = Instantiate(GameManager.game.wallEffectPrefab, transform.position, Quaternion.identity);
            // 壁エフェクトを0.2秒後に消す
            Destroy(wallEffect, 0.5f);

            // プレイヤーの弾を破壊
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Enemy")) // エネミーだったら
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // 出血エフェクトを実体化する
            GameObject bloodEffect = Instantiate(GameManager.game.bloodEffectPrefab, transform.position, Quaternion.identity);
            // 出血エフェクトを0.2秒後に消す
            Destroy(bloodEffect, 0.2f);

            // プレイヤーの弾を破壊
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // 何かが衝突したら（非トリガー）----------------------------------------------------------------------------------------------------------------------------------------
    {
        if (collision.gameObject.CompareTag("Enemy") && !isWallHit)
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // 出血エフェクトを実体化する
            GameObject bloodEffect = Instantiate(GameManager.game.bloodEffectPrefab, transform.position, Quaternion.identity);
            // 出血エフェクトを0.2秒後に消す
            Destroy(bloodEffect, 0.2f);

            // プレイヤーの弾を破壊
            Destroy(this.gameObject);
        }
    }
}