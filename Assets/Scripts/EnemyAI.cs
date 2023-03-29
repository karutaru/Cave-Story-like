using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class EnemyAI : MonoBehaviour
{
    [Header("ステータス")]
    public int hp = 5;
    public int attack = 2;
    public float speed = 40f;
    public float jumpPower = 20f;

    [Header("経路探索")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("パス挙動")]
    public float nextWayPointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;

    [Header("行動の許可")]
    public bool followEnabled = true; //追跡が可能
    public bool jumpEnabled = true; //ジャンプが可能
    public bool directionLookEnabled = true; //方向変化が可能
    public bool walkEnabled = true; //歩行が可能
    public bool stepSound = false; //歩行サウンドを鳴らす
    public bool attackEnabled = false; //攻撃が可能
    public bool flyEnabled = false; //飛行が可能

    [Header("音 関連")]
    public AudioClip walkSE;
    public AudioClip idleSE;
    public AudioClip deathSE;
    public float walkSoundCounter;

    [Header("プレハブ 関連")]
    public GameObject ExplosionFirstPrefab;
    public GameObject ExplosionPrefab;

    [Header("システム")]
    public LayerMask groundLayer;

    //--------------------------動的な変数-------------------------------

    [Header("※変数確認用")]
    public int currentHP;
    public bool canAttack = false;

    //-----------------------------雑多----------------------------------

    private Animator anim;
    Rigidbody2D rb;
    private Path path; //パス
    Seeker seeker; //ターゲットを見る

    private bool isGrounded = false; //接地判定
    private int currentWaypoint = 0;
    private int damage;
    private float jumpTimer;
    private float walkTimer;
    private int moveTimer;
    private bool jumping;
    private bool canStep;
    private float distance;
    private Vector2 direction;
    private BulletController bulletController;

    //-----------------------------ここまで----------------------------------------


    public void Start()
    {
        rb =GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        currentHP = hp;
    }

    void Update()
    {
        isjumpTimer(); //ジャンプしているか
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled) //ターゲットとの距離が近く、追跡できる場合
        {
            PathFollow(); //動きの大きな処理

            MoveStart();//動き出した時
        }

        else {
            MoveStop(); //動かない時
        }


        if (stepSound && canStep) //歩行SEがあり、あるいている時
        {
            WalkSound();
        }
    }

    //--------------------------------------------------------------↓-パス関連-↓----------------------------------------------------------------------

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow() //追跡できる場合
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        
        PathDirection(); //Direction計算へ

        Jump(); //ジャンプへ

        Move(); //移動へ

        Fly(); //飛行へ

        Attack(); //攻撃へ

        NextWayPointDistance(); //次のWaypointの計算へ


        LookDirection(); //見ている方向の処理へ
    }

    //-------------------------------------------------------------------↓-移動関連-↓-------------------------------------------------------------------------


    //移動
    void Move()
    {
        //移動
        if (walkEnabled && Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (anim != null)
            {
                anim.Play("Walk");
                canStep = true;
            }
            //rb.AddForce(force);
            this.gameObject.transform.localPosition = new Vector2 (this.gameObject.transform.localPosition.x + (speed / 1000f) * -transform.localScale.x, this.gameObject.transform.localPosition.y);

        } else //0.04f
        {
            Debug.Log("Idle");
            anim.Play("Idle");
        }
    }

    //動き出した時
    void MoveStart()
    {
        moveTimer = 0;
    }

    //動かない時
    void MoveStop()
    {
        if (anim != null && moveTimer == 0)
                {
                    anim.Play("Idle"); //止まるアニメーション
                    canStep = false;

                }
            if (idleSE != null && moveTimer == 0)
                {
                    AudioSource.PlayClipAtPoint(idleSE, transform.position); //止まるSEを再生
                    canStep = false;
                    moveTimer = 0;
                }
            moveTimer ++;
    }


    //ジャンプ
    void Jump()
    {
        //接地判定
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.5f, Color.red, 0.5f);

        //ジャンプ判定
        if (jumpEnabled && isGrounded && jumping == false)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                jumping = true;

                //ジャンプ実行
                rb.AddForce(transform.up * jumpPower * 10f);
            }
        }
    }

    //足音
    private void WalkSound()
    {
        walkTimer += Time.deltaTime;
        if (walkTimer >= walkSoundCounter)
        {
            AudioSource.PlayClipAtPoint(walkSE, transform.position);
            walkTimer = 0;
        }
    }

    //ジャンプしているか
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

    //飛行
    void Fly()
    {
        if (flyEnabled == true) //飛行が許可されているなら
        {
            //障害物以外を移動できるパス処理
        }
    }

    //見ている方向の処理
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

    void Attack()
    {
        if (canAttack && attackEnabled) //攻撃可能かつ攻撃が許可されている場合
        {
            //エネミーの攻撃処理
        }
    }
    
    //--------------------------------------------ステータス関連-----------------------------------------------

    //プレイヤーの弾がぶつかった時
    void OnTriggerEnter2D(Collider2D col)
    {
        if (bulletController = col.GetComponent<BulletController>())
        {
            damage = bulletController.weaponDamage; //プレイヤーの弾からダメージを持ってくる

            currentHP -= damage; //現在のHPからダメージ分を引く

            if (currentHP <= 0) //現在のHPが0以下になったら
            {
                AudioSource.PlayClipAtPoint(deathSE, transform.position); //死亡時のSE再生

                GameObject ExplosionFirstEffect = Instantiate(ExplosionFirstPrefab, transform.position, Quaternion.identity); //最初の大きな爆発エフェクトを生成
                GameObject ExplosionEffect = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity); //こまかな爆発エフェクトを生成

                Destroy(ExplosionFirstEffect, 0.4f);
                Destroy(ExplosionEffect, 1f);

                Destroy(this.gameObject);
            }
        }
    }

    //-------------------------------------------↓-パス関連-↓-----------------------------------------------

    //追跡対象との距離が近ければtrueを返す
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    //パス完了
    private void OnPathComplete(Path p) {
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
    }

    //パスの高さ？
    void PathDirection()
    {
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
    }

    //次のWayPointの距離？
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

    //DOTweenを止める
    void OnCollisionEnter2D(Collision2D col)
    {
        this.transform.DOKill();
    }
}
