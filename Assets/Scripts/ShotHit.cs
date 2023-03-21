using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ShotHit : MonoBehaviour
{
    public GameObject enemyEffectPrefab;//エネミー用の衝突プレファブ
    public GameObject wallEffectPrefab;//カベ用の衝突プレファブ
    [Header("壁ならチェック、エネミーならいじらない")]
    public bool wallOrEnemy;
    public MMFeedbacks hitDamage; //敵に弾が当たった時

    private void OnTriggerEnter2D(Collider2D other) //なにかがぶつかったら
    {
        if (other.CompareTag("Shell")) //プレイヤーの弾だったら
        {
            Debug.Log("弾が当たった");

            if (other != null) //弾がnullじゃなかったら
            {
                if (wallOrEnemy == true)        //壁なら
                {
                //壁エフェクトを実体化する
                GameObject wallEffect = Instantiate(wallEffectPrefab, other.transform.position, Quaternion.identity);
                //壁エフェクトを0.2秒後に消す
                Destroy(wallEffect, 0.2f);

                } else {                        //エネミーなら

                //hitDamage.PlayFeedbacks();

                //出血エフェクトを実体化する
                GameObject bloodEffect = Instantiate(enemyEffectPrefab, transform.position, Quaternion.identity);
                //出血エフェクトを0.2秒後に消す
                Destroy(bloodEffect, 0.2f);
                }
                //プレイヤーの弾を破壊
                Destroy(other.gameObject);
            }
        }
    }
}
