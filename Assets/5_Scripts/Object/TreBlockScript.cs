using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreBlockScript : MonoBehaviour
{
    [Title("プレハブ設定")]
    [Tooltip("インスタンシエイトするプレハブのリスト")]
    public GameObject[] rockShard;

    [Title("ブロックの耐久値")]
    [Tooltip("ブロックの耐久値")]
    public int treBlock = 100;

    // プレイヤーの弾がぶつかった時
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Shell") && col.TryGetComponent(out BulletController bulletController))
        {
            // treBlock変数の数値をbulletController.WeaponDamageの数値分減らす
            treBlock -= bulletController.WeaponDamage;

            // treBlock変数の数値が0以下になったら
            if (treBlock <= 0)
            {
                // 岩のカケラを飛ばす
                foreach (var shard in rockShard)
                {
                    GameObject shardInstance = Instantiate(shard, transform.position, Quaternion.identity);
                    if (shardInstance.TryGetComponent(out Rigidbody2D rb))
                    {
                        int randomValue = Random.Range(30, 40);
                        int randomUpValue = Random.Range(3, 8);
                        rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                        rb.AddForce((transform.position - bulletController.transform.position).normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
                    }
                }

                // 死亡時のSE再生
                AudioSource.PlayClipAtPoint(DataBase.instance.deathSE, transform.position);

                // 最初の大きな爆発エフェクトを生成
                GameObject ExplosionFirstEffect = Instantiate(DataBase.instance.ExplosionFirstPrefab, transform.position, Quaternion.identity);
                // こまかな爆発エフェクトを生成
                GameObject ExplosionEffect = Instantiate(DataBase.instance.ExplosionPrefab, transform.position, Quaternion.identity);

                // 生成したエフェクトを破壊
                Destroy(ExplosionFirstEffect, 0.4f);
                Destroy(ExplosionEffect, 1f);

                // このオブジェクトを破壊
                Destroy(this.gameObject);
            }
        }
    }
}