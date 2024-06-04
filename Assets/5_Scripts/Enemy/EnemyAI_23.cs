using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using TMPro;

/// <summary>
/// エネミーの行動に関するスクリプト
/// </summary>
public class EnemyAI_23 : EnemyBase
{
    public float duration = 1f; // アニメーションの所要時間
    public float moveY = 2f;    // 頭を上に移動する距離
    public float activateDistanceIsPlayer; // プレイヤーと接触している時の索敵距離

    private bool headMoved = false;
    private bool headStoped = false;
    private float headTran;
    public bool isPlayer;

    private GameObject enemy23_body; // bodyオブジェクトを格納する変数
    private CapsuleCollider2D bodyCollider; // bodyオブジェクトのCapsuleCollider2D

    protected override void Start()
    {
        base.Start();

        Transform headTransform = transform.Find("Head");
        headTran = headTransform.position.y;

        // bodyオブジェクトを取得
        enemy23_body = transform.Find("Body").gameObject;
        if (enemy23_body != null)
        {
            bodyCollider = enemy23_body.GetComponent<CapsuleCollider2D>();
        }
    }

    //-------------------------------------------------------------------↓-移動関連-↓-------------------------------------------------------------------------

    /// <summary>
    /// 移動
    /// </summary>
    protected override void Move()
    {
        //移動
        if (walkEnabled && Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (anim != null && anim.HasState(0, awakeStateHash))
            {
                // 歩くアニメーションを再生
                anim.Play(awakeStateHash);

                if (!headMoved)
                {
                    // 頭の当たり判定を上に移動
                    Transform headTransform = transform.Find("Head");
                    if (headTransform != null)
                    {
                        headTransform.position = new Vector3(headTransform.position.x, headTran, headTransform.position.z);
                        headTransform.DOMoveY(headTransform.position.y + moveY, duration).SetEase(Ease.InOutQuad);
                        headTran = headTransform.position.y + moveY;

                        headMoved = true;
                        headStoped = false;
                    }
                }

                canStep = true;
            }
            //rb.AddForce(force);
            this.gameObject.transform.localPosition = new Vector2(this.gameObject.transform.localPosition.x + (speed / 1000f) * -transform.localScale.x, this.gameObject.transform.localPosition.y);
        }
    }

    protected override void StopMove()
    {
        if (walkEnabled && anim != null && moveTimer == 0 && anim.HasState(0, idleStateHash) && !isPlayer)
        {
            // 止まるアニメーション
            anim.Play("Idle");

            if (!headStoped)
            {
                // 頭の当たり判定を下に移動
                Transform headTransform = transform.Find("Head");
                if (headTransform != null)
                {
                    headTransform.position = new Vector3(headTransform.position.x, headTran, headTransform.position.z);
                    headTransform.DOMoveY(headTransform.position.y - moveY, duration).SetEase(Ease.InExpo);
                    headTran = headTransform.position.y - moveY;

                    headMoved = false;
                    headStoped = true;
                }
            }

            canStep = false;
        }
        if (idleSE != null && moveTimer == 0)
        {
            // 止まるSEを再生
            AudioSource.PlayClipAtPoint(idleSE, transform.position);

            canStep = false;
            moveTimer = 0;
        }
        moveTimer++;
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

    public void ChildCollidedWithPlayer(GameObject child, bool isColliding)
    {
        isPlayer = isColliding;
    }

    protected override bool InDistanceTarget()
    {
        if (isPlayer)
        {
            return Vector2.Distance(transform.position, target.transform.position) < activateDistanceIsPlayer;
        }
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    //-------------------------------------------------------------ステータス関連------------------------------------------------------------------

    //----------------------------------------------------------------↓-パス関連-↓-----------------------------------------------------------------

    //----------------------------------------------------------↓-その他-↓----------------------------------------------------------------------

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        // bodyのCapsuleCollider2DにShellタグが付いたオブジェクトが触れた場合
        if (col.CompareTag("Shell") && col == bodyCollider)
        {
            BulletController bulletController = col.GetComponent<BulletController>();
            if (bulletController != null)
            {
                int damage = bulletController.WeaponDamage;
                // ダメージ処理をここに追加
                currentHP -= damage;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        // bodyのCapsuleCollider2DにShellタグが付いたオブジェクトが触れた場合
        if (col.collider.CompareTag("Shell") && col.collider == bodyCollider)
        {
            BulletController bulletController = col.collider.GetComponent<BulletController>();
            if (bulletController != null)
            {
                int damage = bulletController.WeaponDamage;
                // ダメージ処理をここに追加
                currentHP -= damage;
            }
        }
    }
}