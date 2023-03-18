using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // ジャンプに関するパラメータ
    [SerializeField, Min(0)]
    private float jumpPower = 5f;               //ジャンプ力
    private float jumpPowerKeep;
    private float jumpWater = 1f;               //水中のジャンプ力
    [SerializeField]
    AnimationCurve jumpCurve = new();
    [SerializeField]
    AnimationCurve jumpDownCurve = new();
    [SerializeField, Min(0)]
    private float maxJumpTime = 1f;             //跳躍の限界
    [SerializeField]
    private string JumpButtonName = "Jump";
    private bool jumping = false;               //飛んでいるか
    private bool isRun;                         //走っている
    private Transform thisTransform;
    private float jumpTime = 0;
    private float t;
    private float power;
    private float gravity;

    [SerializeField]
    private Rigidbody2D rb;                      // プレイヤーのRigidbody2D
    private string horizontal = "Horizontal";    // キー入力用の文字列指定(InputManager の Horizontal の入力を判定するための文字列)
    private Animator anim;
    private GameObject headHitEffect;
    private bool headHit;
    private bool headHitOnce = false;            //頭を打つのはジャンプ中１回だけ
    private bool jumpLandingOnce = false;        //着地するのはジャンプ後１回だけ
    public bool inWater;                        //水の中か
    public bool isGrounded;
    private float rotation = 0;                         // 向きの設定に利用する
    public float playerLookDirection = -1f;             // 今のプレイヤーの向き
    private float stepTimer;                     //万歩計のカウント用
    public float moveSpeed;                      //移動速度
    private float moveSpeedKeep;                 //移動速度の保存
    public float knockbackPower;                 // 敵と接触した際に吹き飛ばされる力
    public GameObject headHitEffectPrefab;
    public AudioClip headHitSE;
    public AudioClip jumpLandingSE;
    public AudioClip jumpSE;
    public AudioClip stepSE;

    [SerializeField] 
    CinemachineVirtualCamera artViewCamera;
    CinemachineFramingTransposer artViewCameraTransposer;

    [SerializeField, Header("Linecast用 地面判定レイヤー")]
    private LayerMask groundLayer;


    void Start()
    {
        artViewCameraTransposer = artViewCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.localRotation.x; //0
        thisTransform = transform;
        playerLookDirection = -1f;
    }

    void Update()
    {
        if (isGrounded == true) //接地していたら
        {

            if (inWater == false)
            {
                rb.gravityScale = 4f;
                if (jumpLandingOnce == true && rb.velocity.y <= -8f)
                {
                    AudioSource.PlayClipAtPoint(jumpLandingSE, transform.position);
                    jumpLandingOnce = false;
                }
            }
        }
        else //接地していなかったら
        {
            if (inWater == false)
            {
                rb.gravityScale = 2f;
                jumpLandingOnce = true;
            }
        }

        //接地判定用のラインキャスト
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.1f, transform.position - transform.up * 0.1f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * 0.1f, transform.position - transform.up * 0.1f, Color.red, 1.0f);

        //頭打ち判定用のラインキャスト
        headHit = Physics2D.Linecast(transform.position + transform.up * 1f, transform.position + transform.up * 0.9f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * 1f, transform.position + transform.up * 0.9f, Color.red, 1.0f);

        if (headHit == true) //天井に頭を打ったら
        {
            if (headHitOnce == false)
                {
                    //エフェクト生成
                    headHitEffect = Instantiate(headHitEffectPrefab, new Vector2 (this.transform.position.x, this.transform.position.y + 0.8f), this.transform.rotation);
                    //頭打ちSE再生
                    AudioSource.PlayClipAtPoint(headHitSE, transform.position);
                    headHitOnce = true;
                }
            headHit = false;
            jumping = false;
            jumpTime = 0;
            //下に飛ぶ
            rb.AddForce(power * -Vector2.up, (ForceMode2D)ForceMode.Impulse);
        }


        // ジャンプの開始判定
        if (isGrounded && Input.GetButtonDown(JumpButtonName)) //接地していて、かつジャンプボタンを押した時
        {
            anim.Play("Player_Jump");
            //ジャンプSE再生
            AudioSource.PlayClipAtPoint(jumpSE, transform.position);
            //飛んでいる
            jumping = true;
            //頭をぶつけられる
            headHitOnce = false;
        }


        if (jumping)                                             // ジャンプ中の処理◆◆
        {
            if (Input.GetButtonUp(JumpButtonName) || jumpTime >= maxJumpTime) //ジャンプボタンを離したら
            {
                jumping = false;
                rb.AddForce(-Vector2.up / (jumpTime + 0.1f) / jumpWater, (ForceMode2D)ForceMode.Impulse);
                jumpTime = 0;
            }
            else if(Input.GetButton(JumpButtonName)) //ジャンプボタンを押し続けている間
            {
                jumpTime += Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) //上矢印キー、もしくはWキーを押している
        {

            if (isRun == true) //走っている最中なら
            {
                anim.Play("LookUp");
                anim.SetFloat("LookUpIdle", 0.0f);
            }
            else { //走っていないなら
                anim.Play("LookUpIdle");
                anim.SetFloat("LookUp", 0.0f);
            }
        }
        else { //上キーを押していない状態

            anim.SetFloat("LookUpIdle", 0.0f);
            anim.SetFloat("LookUp", 0.0f);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) //上矢印キー、もしくはWキーを離したら
        {
            if (jumping == true) //飛んでる
            {
                anim.SetFloat("LookUpIdle", 0.0f);
                anim.SetFloat("LookUp", 0.0f);
                anim.Play("Player_Jump");
            } else { //飛んでない
                anim.Play("Player_Idle");
            }
        }
    }

    void FixedUpdate()
    {
        //移動
        Move();
        //ジャンプ
        Jump();

    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()                                           //移動◆◆
    {
        // 水平方向への入力受付
        float x = Input.GetAxis(horizontal);

        if (x != 0)
        {
            //走っている時
            isRun = true;
            //歩いている間加算
            stepTimer += Time.deltaTime;

            if (stepTimer >= 0.3f && isGrounded == true) //一定距離歩き、かつ接地している時
            {
                //歩きSE再生
                AudioSource.PlayClipAtPoint(stepSE, transform.position);

                stepTimer = 0;
            }

            // x の値が 0 ではない場合 = キー入力がある場合
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);
            // temp 変数に現在の Rotation 値を代入
            Vector3 temp = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
            // 現在のキー入力値 x を temp.x に代入
            temp.x = x;

            //整数値にする
            if (temp.x > 0)
            {
                //0よりも大きければ1にする
                this.transform.rotation = Quaternion.Euler(0f, 180f, 0f); //右向き
                playerLookDirection = 1f;
            }
            else
            {
                //0よりも小さければ-1にする
                this.transform.rotation = Quaternion.Euler(0f, 0f, 0f); //左向き
                playerLookDirection = -1f;
            }
            //キャラの向きを移動方向に合わせる
            // transform.localScale = temp;

            if (isGrounded == true) //飛んでない
            {
                //待機状態のアニメの再生を止めて、走るアニメの再生への遷移を行う
                anim.SetFloat("Run", 0.2f);
            }
            if (isGrounded == false)
            {
                anim.SetFloat("Run", 0.0f);
            }

        }
        else
        {
            //走っていない時
            isRun = false;
            // 左右の入力がなかったら横移動の速度を0にしてすぐに停止する
            rb.velocity = new Vector2(0, rb.velocity.y);
            // 走るアニメの再生を止めて、待機状態のアニメの再生への遷移を行う
            anim.SetFloat("Run", 0.0f);
            // velocity(速度)に新しい値を代入して移動
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


    void Jump()                                             //ジャンプ◆◆
    {
        if (!jumping) //飛んでない
        {
            //止める
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

        //ジャンプの速度をアニメーションカーブから取得
        float t = jumpTime / maxJumpTime;
        float power = jumpPower * jumpCurve.Evaluate(t);

        if (t >= 1.0) //ジャンプ時間の限界以上なら
        {
            //ジャンプを止める
            jumping = false;
            jumpTime = 0;
        }

        //上に飛ぶ
        rb.AddForce(power * Vector2.up, (ForceMode2D)ForceMode.Impulse);
    }


    private void OnCollisionEnter2D(Collision2D col) { //接触したなら

        // 接触したコライダーを持つゲームオブジェクトのTagがEnemyなら 
        if (col.gameObject.tag == "Enemy") {

            // キャラと敵の位置から距離と方向を計算
            Vector3 direction = (transform.position - col.transform.position).normalized;

            // 敵の反対側にキャラを吹き飛ばす
            transform.position += direction * knockbackPower;
        }
    }

    public void WaterMove(bool waterIn)                     //水の中◆◆
    {
        inWater = waterIn;
        if (inWater == true) //水に入ったら
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.1f);
            //ジャンプ力を保存
            jumpPowerKeep = jumpPower;
            //ジャンプを2分の1にする
            jumpPower = jumpPower / 2f;
            jumpWater = 1000f;
            rb.gravityScale = 1f;
            moveSpeedKeep = moveSpeed;
            moveSpeed = moveSpeed / 2;

        } else { //水から出たら
            jumpPower = jumpPowerKeep;
            jumpWater = 1f;
            rb.gravityScale = 2f;
            moveSpeed = moveSpeedKeep;
        }
    }
}
