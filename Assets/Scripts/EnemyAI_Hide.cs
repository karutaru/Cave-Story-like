using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using UnityEngine.UIElements;

/// <summary>
/// エネミーの行動に関するスクリプト
/// </summary>
public class EnemyAI_Hide : EnemyBase
{
    private int awakeCounter;
    private int waitCounter;
    private bool isAwake;

    //-----------------------------------------------------ここまで---------------------------------------------------------------


    protected override void Start()
    {
        if (!TryGetComponent(out rb)) return;
        if (!TryGetComponent(out anim)) return;
        if (!TryGetComponent(out seeker)) return;


        walkEnabled = false;
        anim.Play("Idle");

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //現在のHPを最大値まで回復
        currentHP = hp;
    }

    protected override void Update()
    {
        // 起きた判定
        IsAwake();

        // ジャンプの判定
        isjumpTimer();

        // 死亡判定
        OrDeath();


    }

    protected override void FixedUpdate()
    {
        // ターゲットとの距離が近く、追跡できる場合
        if (InDistanceTarget() && followEnabled)
        {
            // 移動先決定
            FollowPath();

            // 移動開始
            StartMove();
        }

        else
        {
            // 移動停止
            StopMove();
        }

        // 歩行SEがあり、歩いている時
        if (stepSound && canStep)
        {
            // 歩くSEを鳴らす
            IsWalkSound();
        }
    }

    //-------------------------------------------------------------------↓-移動関連-↓-------------------------------------------------------------------------

    void IsAwake()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (awakeCounter == 0)
            {
                anim.Play("Awake");
                StartCoroutine("AwakeUp");
            }
            if (awakeCounter == 0)
            {
                awakeCounter++;
            }
            waitCounter = 0;
        }
        else if (isAwake)
        {
            if (waitCounter == 0)
            {
                isAwake = false;
                walkEnabled = false;
                anim.Play("Sleep");
            }
            if (waitCounter == 0)
            {
                waitCounter++;
            }
            awakeCounter = 0;
        }
    }
    IEnumerator AwakeUp()
    {
        //0.5秒停止
        yield return new WaitForSeconds(1f);

        walkEnabled = true;
        isAwake = true;


    }

    /// <summary>
    /// 移動
    /// </summary>
    protected override void Move()
    {
        //移動
        if (walkEnabled && Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (anim != null)
            {
                // 歩くアニメーションを再生
                if (walkEnabled)
                {
                    anim.Play("Walk");
                }

                canStep = true;
            }
            //rb.AddForce(force);
            this.gameObject.transform.localPosition = new Vector2(this.gameObject.transform.localPosition.x + (speed / 1000f) * -transform.localScale.x, this.gameObject.transform.localPosition.y);

        }
        //else if (walkEnabled) //0.04f
        //{
        //    // 立ち止まるアニメーションを再生
        //    if (walkEnabled)
        //    {
        //        anim.Play("Idle");
        //    }
        //}
    }

    /// <summary>
    /// 動き出し
    /// </summary>
    protected override void StartMove()
    {
        moveTimer = 0;
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    protected override void StopMove()
    {
        if (anim != null && moveTimer == 0)
        {
            // 止まるアニメーション
            if (walkEnabled)
            {
                anim.Play("Idle");
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


    /// <summary>
    /// ジャンプ
    /// </summary>
    protected override void Jump()
    {
        // 接地判定
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.5f, Color.red, 0.5f);

        // ジャンプ判定
        if (jumpEnabled && isGrounded && jumping == false)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                jumping = true;

                // ジャンプ実行
                rb.AddForce(transform.up * jumpPower * 10f);
            }
        }
    }

    /// <summary>
    /// 足音
    /// </summary>
    protected override  void IsWalkSound()
    {
        walkTimer += Time.deltaTime;
        if (walkTimer >= walkSoundCounter)
        {
            AudioSource.PlayClipAtPoint(walkSE, transform.position);
            walkTimer = 0;
        }
    }

    /// <summary>
    /// ジャンプしているか
    /// </summary>
    protected override void isjumpTimer()
    {
        if (jumping == true)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= 0.3f)
            {
                jumpTimer = 0;
                jumping = false;
            }
        }
    }

    //--------------------------------------------------------------↓-固有の行動-↓-----------------------------------------------------------------

    /// <summary>
    /// 攻撃関連
    /// </summary>
    protected override void Attack()
    {

    }

    //-------------------------------------------------------------ステータス関連------------------------------------------------------------------

    // プレイヤーの弾がぶつかった時
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (bulletController = col.GetComponent<BulletController>())
        {
            // プレイヤーの弾からダメージを持ってくる
            damage = bulletController.weaponDamage;

            // 現在のHPからダメージ分を引く
            currentHP -= damage;
        }
    }

    //----------------------------------------------------------------↓-パス関連-↓-----------------------------------------------------------------



    //----------------------------------------------------------↓-その他-↓----------------------------------------------------------------------


}
