using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [Header("ステータス")]
    public int hp = 5;                          // 最大HP
    public int attack = 2;                      // 攻撃力
    public float speed = 40f;                   // 足の速さ
    public float jumpPower = 20f;               // ジャンプ力
    public int exp = 1;                         // 経験値

    [Header("経路探索")]
    public Transform target;                    // ターゲットの位置
    public float activateDistance = 50f;        // 索敵距離
    public float pathUpdateSeconds = 0.5f;      //パスの更新時間

    [Header("パス挙動")]
    public float nextWayPointDistance = 0.5f;
    public float jumpNodeHeightRequirement = 0.8f;

    [Header("行動の許可")]
    public bool followEnabled = true;           // 追跡が可能
    public bool jumpEnabled = true;             // ジャンプが可能
    public bool directionLookEnabled = true;    // 方向変化が可能
    public bool walkEnabled = true;             // 歩行が可能
    public bool stepSound = false;              // 歩行サウンドを鳴らす
    public bool attackEnabled = false;          // 攻撃が可能
    public bool flyEnabled = false;             // 飛行が可能
    [Header("素材の有無")]
    public bool walkAnimEnable = false;         // 歩くアニメの有無
    public bool walkSoundEnable = false;        // 歩く音の有無
    public bool idleAnimEnable = false;         // 止まるアニメの有無
    public bool idleSoundEnable = false;        // 止まる音の有無
    public bool attackSoundEnable = false;      // 攻撃音の有無

    [Header("音 関連")]
    public AudioClip walkSE;                    // 歩くSE
    public AudioClip idleSE;                    // 立ち止まるSE
    public AudioClip deathSE;                   // 死亡時のSE
    public float walkSoundCounter;              // 足音の鳴る間隔

    [Header("プレハブ 関連")]
    public GameObject expPrefabs;               // 経験値のプレハブ
    public GameObject damagePrefabs;            // ダメージテキストのプレハブ
    public GameObject ExplosionFirstPrefab;     // 最初の大きな爆発のプレハブ
    public GameObject ExplosionPrefab;          // 次点の細かな爆発のプレハブ

    [Header("システム")]
    public LayerMask groundLayer;               // 接地判定用のレイヤー

    //--------------------------動的な変数-------------------------------

    [Header("※変数確認用")]
    public int currentHP;                       // 現在のHP
    public bool canAttack = false;              // 攻撃が可能か

    //-----------------------------雑多----------------------------------

    protected Seeker seeker;
    protected Rigidbody2D rb;
    protected Path path;
    protected Animator anim;

    protected float distance;
    protected Vector2 force;
    protected float jumpTimer;
    protected float walkTimer;
    protected int damage;
    protected int moveTimer;
    protected int currentWaypoint = 0;
    protected bool canStep;
    protected bool jumping;
    protected bool isGrounded = false; // 接地判定
    protected Vector2 direction;
    protected BulletController bulletController;


    protected virtual void Start()
    {
        if (!TryGetComponent(out rb)) return;
        if (!TryGetComponent(out anim)) return;
        if (!TryGetComponent(out seeker)) return;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //現在のHPを最大値まで回復
        currentHP = hp;
    }

    protected virtual void Update()
    {
        // ジャンプの判定
        isjumpTimer();

        // 死亡判定
        OrDeath();
    }

    protected virtual void FixedUpdate()
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

    //---------------------------------------------------------------↓-パス関連-↓-----------------------------------------------------------------------

    /// <summary>
    /// パスの更新
    /// </summary>
    protected virtual void UpdatePath()
    {
        if (followEnabled && InDistanceTarget() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// 追跡可能
    /// </summary>
    protected virtual void FollowPath()
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
    protected virtual void Move()
    {
        // 子供のクラスで上書きして使う
    }

    /// <summary>
    /// 動き出し
    /// </summary>
    protected virtual void StartMove()
    {
        moveTimer = 0;
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    protected virtual void StopMove()
    {
        if (walkEnabled && anim != null && moveTimer == 0 && idleAnimEnable)
        {
            // 止まるアニメーション
            anim.Play("Idle");

            canStep = false;

        }
        if (idleSE != null && moveTimer == 0 && idleSoundEnable)
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
    protected virtual void Jump()
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
    protected virtual void IsWalkSound()
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
    protected virtual void isjumpTimer()
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
    protected virtual void Fly()
    {
        
    }

    /// <summary>
    /// 見ている方向の処理
    /// </summary>
    protected virtual void LookDirection()
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

    //--------------------------------------------------------------↓-固有の行動-↓-----------------------------------------------------------------

    /// <summary>
    /// 攻撃関連
    /// </summary>
    protected virtual void Attack()
    {

    }

    //-------------------------------------------------------------ステータス関連------------------------------------------------------------------

    // プレイヤーの弾がぶつかった時
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //if (bulletController == col.GetComponent<BulletController>())
        if (col.TryGetComponent(out BulletController bulletController))
        {
            // プレイヤーの弾からダメージを持ってくる
            damage = bulletController.WeaponDamage;

            // ダメージをポップアップ
            GameObject damageTextObject = Instantiate(damagePrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            Debug.Log(damageTextObject.name);
            damageTextObject.GetComponent<TMP_Text>().text = damage.ToString();

            // プレイヤーの弾が当たった位置との差分を計算
            Vector2 hitDiff = transform.position - bulletController.transform.position;
            // テキストをプレイヤーの弾が当たった位置とは逆方向に向かってポップアップするように、AddForceで力を加える
            //Rigidbody2D textRb = damageTextObject.GetComponent<Rigidbody2D>();
            //textRb.AddForce(hitDiff.normalized * 50f, ForceMode2D.Impulse);

            if (damageTextObject.TryGetComponent(out Rigidbody2D rb))
            {
                int randomValue = Random.Range(30, 40);
                int randomUpValue = Random.Range(3, 8);
                rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                rb.AddForce(hitDiff.normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
            }




            // 現在のHPからダメージ分を引く
            currentHP -= damage;
        }
    }

    protected virtual void OrDeath()
    {
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

            // Expを生成
            ExpScript expScript;

            GameObject expObject = Instantiate(expPrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

            // Expに経験値の情報を送る
            expScript = expObject.GetComponent<ExpScript>();
            expScript.expValue = exp;

            // Expを生成
            GameObject expObject_2 = Instantiate(expPrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

            // Expに経験値の情報を送る
            expScript = expObject_2.GetComponent<ExpScript>();
            expScript.expValue = exp;


            // このオブジェクトを破壊
            Destroy(this.gameObject);
        }
    }

    //----------------------------------------------------------------↓-パス関連-↓-----------------------------------------------------------------

    /// <summary>
    /// 追跡対象との距離が近ければtrueを返す
    /// </summary>
    /// <returns></returns>
    protected virtual bool InDistanceTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    /// <summary>
    /// パス完了
    /// </summary>
    /// <param name="p"></param>
    protected virtual void OnPathComplete(Path p)
    {
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
    }

    /// <summary>
    /// パスの高さ
    /// </summary>
    protected virtual void PathDirection()
    {
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// 次のWayPointの距離
    /// </summary>
    protected virtual void NextWayPointDistance()
    {
        //Next Waypoint
        distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    //----------------------------------------------------------↓-その他-↓----------------------------------------------------------------------

    // DOTweenを止める
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        this.transform.DOKill();
    }


}
