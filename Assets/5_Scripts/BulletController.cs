using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool wallHit = true;
    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // プロパティ

    public void Shoot(Vector2 direction, int maxDamage, int minDamage)
    {
        // 最大値と最小値からダメージを計算し、ダメージセット
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // 発射
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other) // なにかがぶつかったら
    {
        if (other.CompareTag("Obstacle")) // 障害物だったら
        {
            // 壁エフェクトを実体化する
            GameObject wallEffect = Instantiate(GameManager.game.wallEffectPrefab, transform.position, Quaternion.identity);
            // 壁エフェクトを0.2秒後に消す
            Destroy(wallEffect, 0.2f);

            // bulletにアタッチされているEffectScriptのOnDestroyメソッドを呼び出す
            EffectScript effectScript = this.GetComponent<EffectScript>();
            if (effectScript != null)
            {
                effectScript.OnDestroy();
            }

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
}
