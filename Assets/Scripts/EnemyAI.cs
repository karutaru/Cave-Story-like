using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

/// <summary>
/// エネミーの行動に関するスクリプト
/// </summary>
public class EnemyAI : EnemyBase
{

    //-------------------------------------------------------------------↓-移動関連-↓-------------------------------------------------------------------------


    /// <summary>
    /// 移動
    /// </summary>
    protected override void Move()
    {
        //移動
        if (walkEnabled && Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (anim != null && walkAnimEnable)
            {
                // 歩くアニメーションを再生
                anim.Play("Walk");

                canStep = true;
            }
            //rb.AddForce(force);
            this.gameObject.transform.localPosition = new Vector2 (this.gameObject.transform.localPosition.x + (speed / 1000f) * -transform.localScale.x, this.gameObject.transform.localPosition.y);

        } else if (idleAnimEnable)
        {
            // 立ち止まるアニメーションを再生
            anim.Play("Idle");
        }
    }

    //--------------------------------------------------------------↓-固有の行動-↓-----------------------------------------------------------------

    /// <summary>
    /// 攻撃関連
    /// </summary>
    protected override void Attack()
    {
        // 攻撃可能かつ攻撃が許可されている場合
        if (canAttack && attackEnabled)
        {
            // TODO エネミーの攻撃処理
        }
    }
    
    //-------------------------------------------------------------ステータス関連------------------------------------------------------------------



    //----------------------------------------------------------------↓-パス関連-↓-----------------------------------------------------------------



    //----------------------------------------------------------↓-その他-↓----------------------------------------------------------------------
}
