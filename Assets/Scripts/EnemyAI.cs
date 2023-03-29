using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

/// <summary>
/// エネミーの行動に関するスクリプト
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("ステータス")]
    public int hp = 5;                          // 最大HP
    public int attack = 2;                      // 攻撃力
    public float speed = 40f;                   // 足の速さ
    public float jumpPower = 20f;               // ジャンプ力

    [Header("経路探索")]
    public Transform target;                    // ターゲットの位置
    public float activateDistance = 50f;        // 索敵距離
    public float pathUpdateSeconds = 0.5f;      //パスの更新時間

    [Header("パス挙動")]
    public float nextWayPointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;

    [Header("行動の許可")]
    public bool followEnabled = true;           // 追跡が可能
    public bool jumpEnabled = true;             // ジャンプが可能
    public bool directionLookEnabled = true;    // 方向変化が可能
    public bool walkEnabled = true;             // 歩行が可能
    public bool stepSound = false;              // 歩行サウンドを鳴らす
    public bool attackEnabled = false;          // 攻撃が可能
    public bool flyEnabled = false;             // 飛行が可能

    [Header("音 関連")]
    public AudioClip walkSE;                    // 歩くSE
    public AudioClip idleSE;                    // 立ち止まるSE
    public AudioClip deathSE;                   // 死亡時のSE
    public float walkSoundCounter;              // 足音の鳴る感覚

    [Header("プレハブ 関連")]
    public GameObject ExplosionFirstPrefab;     // 最初の大きな爆発のプレハブ
    public GameObject ExplosionPrefab;          // 次点の細かな爆発のプレハブ

    [Header("システム")]
    public LayerMask groundLayer;               // 接地判定用のレイヤー

    //--------------------------動的な変数-------------------------------

    [Header("※変数確認用")]
    public int currentHP;                       // 現在のHP
    public bool canAttack = false;              // 攻撃が可能か

    //-----------------------------雑多----------------------------------

    Seeker seeker;
    Rigidbody2D rb;
    private Path path;
    private Animator anim;

    private float distance;
    private float jumpTimer;
    private float walkTimer;
    private int damage;
    private int moveTimer;
    private int currentWaypoint = 0;
    private bool canStep;
    private bool jumping;
    private bool isGrounded = false; // 接地判定
    private Vector2 direction;
    private BulletController bulletController;

    //-----------------------------ここまで----------------------------------------


    public void Start()
    {
        if(!TryGetComponent(out rb)) return;
        if(!TryGetComponent(out anim)) return;
        if(!TryGetComponent(out seeker)) return;
 
        Debug.Log("各変数の取得完了");

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //現在のHPを最大値まで回復
        currentHP = hp;
    }

    void Update()
    {
        // ジャンプの判定
        isjumpTimer();
    }

    private void FixedUpdate()
    {
        // ターゲットとの距離が近く、追跡できる場合
        if (InDistanceTarget() && followEnabled)
        {
            // 移動先決定
            FollowPath();

            // 移動開始
            StartMove();
        }

        else {
            // 移動停止
            StopMove();
        }

        // 歩行SEがあり、あるいている時
        if (stepSound && canStep)
        {
            // 歩くSEを鳴らす
            IsWalkSound();
        }
    }

    //--------------------------------------------------------------↓-パス関連-↓----------------------------------------------------------------------

    /// <summary>
    /// パスの更新
    /// </summary>
    private void UpdatePath()
    {
        if (followEnabled && InDistanceTarget() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// 追跡可能
    /// </summary>
    private void FollowPath()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        
        // Direction計算へ
        PathDirection();

        // ジャンプへ
        Jump();

        // 移動へ
        Move();

        // 飛行へ
        Fly();

        // 攻撃へ
        Attack();

        // 次のWaypointの計算へ
        NextWayPointDistance();

        // 見ている方向の処理へ
        LookDirection();
    }

    //-------------------------------------------------------------------↓-移動関連-↓-------------------------------------------------------------------------


    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        //移動
        if (walkEnabled && Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (anim != null)
            {
                // 歩くアニメーションを再生
                anim.Play("Walk");

                canStep = true;
            }
            //rb.AddForce(force);
            this.gameObject.transform.localPosition = new Vector2 (this.gameObject.transform.localPosition.x + (speed / 1000f) * -transform.localScale.x, this.gameObject.transform.localPosition.y);

        } else //0.04f
        {
            // 立ち止まるアニメーションを再生
            anim.Play("Idle");
        }
    }

    /// <summary>
    /// 動き出し
    /// </summary>
    void StartMove()
    {
        moveTimer = 0;
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    void StopMove()
    {
        if (anim != null && moveTimer == 0)
                {
                    // 止まるアニメーション
                    anim.Play("Idle");

                    canStep = false;

                }
            if (idleSE != null && moveTimer == 0)
                {
                    // 止まるSEを再生
                    AudioSource.PlayClipAtPoint(idleSE, transform.position);

                    canStep = false;
                    moveTimer = 0;
                }
            moveTimer ++;
    }


    /// <summary>
    /// ジャンプ
    /// </summary>
    void Jump()
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
    private void IsWalkSound()
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
    void isjumpTimer()
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

    /// <summary>
    /// 飛行
    /// </summary>
    void Fly()
    {
        // 飛行が許可されているなら
        if (flyEnabled == true)
        {
            // 障害物以外を移動できるパス処理
        }
    }

    /// <summary>
    /// 見ている方向の処理
    /// </summary>
    void LookDirection()
    {
        if (directionLookEnabled)
        {
            if (direction.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    //------------------------------------------↓-固有の行動-↓----------------------------------------------

    /// <summary>
    /// 攻撃関連
    /// </summary>
    void Attack()
    {
        // 攻撃可能かつ攻撃が許可されている場合
        if (canAttack && attackEnabled)
        {
            // エネミーの攻撃処理
        }
    }
    
    //--------------------------------------------ステータス関連-----------------------------------------------

    // プレイヤーの弾がぶつかった時
    void OnTriggerEnter2D(Collider2D col)
    {
        if (bulletController = col.GetComponent<BulletController>())
        {
            // プレイヤーの弾からダメージを持ってくる
            damage = bulletController.weaponDamage;

            // 現在のHPからダメージ分を引く
            currentHP -= damage;

            // 現在のHPが0以下になったら
            if (currentHP <= 0)
            {
                // 死亡時のSE再生
                AudioSource.PlayClipAtPoint(deathSE, transform.position);

                // 最初の大きな爆発エフェクトを生成
                GameObject ExplosionFirstEffect = Instantiate(ExplosionFirstPrefab, transform.position, Quaternion.identity);
                // こまかな爆発エフェクトを生成
                GameObject ExplosionEffect = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

                // 生成したエフェクトを破壊
                Destroy(ExplosionFirstEffect, 0.4f);
                Destroy(ExplosionEffect, 1f);

                // このオブジェクトを破壊
                Destroy(this.gameObject);
            }
        }
    }

    //-------------------------------------------↓-パス関連-↓-----------------------------------------------

    /// <summary>
    /// 追跡対象との距離が近ければtrueを返す
    /// </summary>
    /// <returns></returns>
    private bool InDistanceTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    /// <summary>
    /// パス完了
    /// </summary>
    /// <param name="p"></param>
    private void OnPathComplete(Path p) {
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
    }

    /// <summary>
    /// パスの高さ？
    /// </summary>
    void PathDirection()
    {
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// 次のWayPointの距離？
    /// </summary>
    void NextWayPointDistance()
    {
        //Next Waypoint
        distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    //---------------------------------------↓-その他-↓--------------------------------------------------

    // DOTweenを止める
    void OnCollisionEnter2D(Collision2D col)
    {
        this.transform.DOKill();
    }
}
