using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreBlockScript : MonoBehaviour
{
    public GameObject rockShard;


    // プレイヤーの弾がぶつかった時
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //if (bulletController == col.GetComponent<BulletController>())
        if (col.TryGetComponent(out BulletController bulletController))
        {
            // プレイヤーの弾が当たった位置との差分を計算
            Vector2 hitDiff = transform.position - bulletController.transform.position;

            // 岩のカケラを飛ばす
            if (rockShard.TryGetComponent(out Rigidbody2D rb))
            {
                int randomValue = Random.Range(30, 40);
                int randomUpValue = Random.Range(3, 8);
                rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                rb.AddForce(hitDiff.normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
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
