using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_23_isPlayer : MonoBehaviour
{
    private EnemyAI_23 enemyAI_23;


    void Start()
    {
        // 親オブジェクトのスクリプトを取得
        enemyAI_23 = GetComponentInParent<EnemyAI_23>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        PlayerBodyController playerBodyController = col.gameObject.GetComponent<PlayerBodyController>();

        if (playerBodyController != null)
        {
            enemyAI_23.ChildCollidedWithPlayer(this.gameObject, true);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        PlayerBodyController playerBodyController = col.gameObject.GetComponent<PlayerBodyController>();

        if (playerBodyController != null)
        {
            enemyAI_23.ChildCollidedWithPlayer(this.gameObject, false);
        }
    }
}
