using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Sirenix.OdinInspector.Editor.Internal.FastDeepCopier;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using AIE2D;

public class PlayerBodyController : MonoBehaviour
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
    public bool isJump = true;                  // ジャンプできるか
    private bool jumping = false;               //飛んでいるか
    private bool jumpingKeep = false;           //飛んで後の空中にいる間true
    private bool isRun;                         //走っている
    private Transform thisTransform;
    private float jumpTime = 0;
    private float t;
    private float power;
    private float gravity;
    public bool isMove;
    private float playerDash; // ダッシュ速度


    private Rigidbody2D rb;                         // プレイヤーのRigidbody2D
    private SpriteRenderer spriteRenderer;
    private string horizontal = "Horizontal";       // キー入力用の文字列指定(InputManager の Horizontal の入力を判定するための文字列)
    private GameObject headHitEffect;
    private bool headHit;
    private bool headHitOnce = false;               //頭を打つのはジャンプ中１回だけ
    private bool jumpLandingOnce = false;           //着地するのはジャンプ後１回だけ
    public bool inWater;                            //水の中か
    public bool isGrounded;
    private float rotation = 0;                     // 向きの設定に利用する
    public float playerLookDirection = -1f;         // 今のプレイヤーの向き
    private float stepTimer;                        // 万歩計のカウント用
    public float defaultMoveSpeed = 3.7f;           // プレイヤーの移動速度
    private float moveSpeed;                         // 移動速度の下限速に用いる数値
    private float moveSpeedKeep;                    // 移動速度の保存
    public float knockbackPower;                    // 敵と接触した際に吹き飛ばされる力
    public GameObject headHitEffectPrefab;
    private bool wasGroundedLastFrame = false;      // 前のフレームでの接地状態を記録
    private bool dodging;                           // 回避中か

    private Collider2D playerCollider;
    private List<Collider2D> enemyColliders = new List<Collider2D>();

    public StaticAfterImageEffect2DPlayer staticAfterImageEffect2DPlayer;

    public AudioClip headHitSE;
    public AudioClip jumpLandingSE;
    public AudioClip jumpSE;
    public AudioClip stepSE;

    public Knockback_Player knockback_Player;
    private Animator anim;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        rotation = transform.localRotation.x; //0
        thisTransform = transform;
        playerLookDirection = -1f;
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isGrounded == true) //接地していたら
        {

            if (inWater == false)
            {
                jumpingKeep = false;
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
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.4f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.4f, transform.position - transform.up * 0.6f, Color.red, 0.3f);

        //頭打ち判定用のラインキャスト
        headHit = Physics2D.Linecast(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.4f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.4f, Color.red, 0.3f);

        // 地面に付いた瞬間にアイドルアニメーションを再生
        if (isGrounded && !wasGroundedLastFrame)
        {
            anim.Play("Player_Idle");
        }

        // 現在の接地状態を次のフレームのために記録
        wasGroundedLastFrame = isGrounded;

        if (headHit == true) //天井に頭を打ったら
        {
            if (headHitOnce == false)
            {
                //エフェクト生成
                headHitEffect = Instantiate(headHitEffectPrefab, new Vector2(this.transform.position.x, this.transform.position.y + 0.3f), this.transform.rotation);
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

        if (Input.GetMouseButtonDown(1) && !isMove)
        {
            isMove = true; // 移動禁止
            staticAfterImageEffect2DPlayer.enabled = true; // 残像の表示
            SetIsMoveFalseAfterDelay().Forget();
            Dodge();
        }

        // ジャンプの開始判定
        if (isGrounded && Input.GetButtonDown(JumpButtonName) && isJump) //接地していて、かつジャンプボタンを押した時にジャンプが許可されていた場合
        {
            if (Time.timeScale == 1)
            {
                anim.Play("Player_Jump");

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //Sか↓を押した時
                {

                }
                else
                {
                    //anim.Play("Player_Jump");
                }
                //ジャンプSE再生
                AudioSource.PlayClipAtPoint(jumpSE, transform.position);
                //飛んでいる
                jumping = true;
                //頭をぶつけられる
                headHitOnce = false;
            }
        }

        // ジャンプが終了し、y軸の速度が負の値になった場合に落下アニメーションを再生
        if (!jumpingKeep && rb.velocity.y < 0)
        {
            anim.Play("Player_Fall");
        }

        if (!jumping && !dodging)
        {
            PlayerAnim();
        }


        if (jumping)                                             // ジャンプ中の処理◆◆
        {
            if (Input.GetButtonUp(JumpButtonName) || jumpTime >= maxJumpTime) //ジャンプボタンを離したら
            {
                jumping = false;
                rb.AddForce(-Vector2.up / (jumpTime + 0.1f) / jumpWater, (ForceMode2D)ForceMode.Impulse);
                jumpTime = 0;
            }
            else if (Input.GetButton(JumpButtonName)) //ジャンプボタンを押し続けている間
            {
                jumpingKeep = true;
                jumpTime += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //上矢印キー、もしくはWキーを押している
        {
            if (Time.timeScale == 1)
            {
                if (isRun == true) //走っている最中なら
                {

                }
                else
                { //走っていないなら

                }
            }
        }
        else
        { //上キーを押していない状態

        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) //上矢印キー、もしくはWキーを離したら
        {
            if (jumping == true) //飛んでる
            {

            }
            else
            { //飛んでない

            }
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.timeScale == 1)
            {

            }
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (jumping == true) //飛んでる
            {

            }
            else
            { //飛んでない

            }
        }
    }

    void FixedUpdate()
    {
        // 移動
        Move();
        // ジャンプ
        Jump();
        // 主人公の向き
        Player_Direction();

    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()                                           //移動◆◆
    {
        if (isMove)
        {
            return;
        }
        // 水平方向への入力受付
        float x = Input.GetAxis(horizontal);

        if (x != 0)
        {
            //走っている時
            isRun = true;
            //歩いている間加算
            stepTimer += Time.deltaTime;

            if (stepTimer >= 0.3f && isGrounded == true && Input.GetKey(KeyCode.LeftShift)) //一定距離歩き、かつ接地していてLシフトを押している時
            {
                //歩きSE再生
                AudioSource.PlayClipAtPoint(stepSE, transform.position);

                stepTimer = 0;
            }

            if (stepTimer >= 0.47f && isGrounded == true) //一定距離歩き、かつ接地している時
            {
                //歩きSE再生
                AudioSource.PlayClipAtPoint(stepSE, transform.position);

                stepTimer = 0;
            }

            float targetVelocityX = x * moveSpeed;


            // 現在の速度を目標速度に向けて徐々に変更
            float newVelocityX = Mathf.MoveTowards(rb.velocity.x, targetVelocityX, 20 * Time.deltaTime);

            // Rigidbodyの速度を更新
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);


            // temp 変数に現在の Rotation 値を代入
            Vector3 temp = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
            // 現在のキー入力値 x を temp.x に代入
            temp.x = x;

            //整数値にする
            if (temp.x > 0)
            {
                playerLookDirection = 1f;
            }
            else
            {
                playerLookDirection = -1f;
            }

            if (isGrounded == true) //飛んでない
            {
                
            }
            if (isGrounded == false)
            {

            }

        }
        else
        {
            //走っていない時
            isRun = false;
        }
    }


    void Jump()                                             //ジャンプ◆◆
    {
        if (Time.timeScale == 1)
        {
            if (!jumping)
            {
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
    }

    void Dodge()
    {
        dodging = true;
        anim.Play("Player_Dodge");
        rb.velocity = new Vector2(0, rb.velocity.y); // xの移動速度を一瞬0にリセット

        if (Input.GetKey(KeyCode.A)) // 左キーを押している場合
        {
            rb.AddForce(Vector2.left * 15, ForceMode2D.Impulse); // 左に弾き飛ばす
        }
        else if (Input.GetKey(KeyCode.D)) // 右キーを押している場合
        {
            rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse); // 右に弾き飛ばす
        }
        else      // 何も押していない場合
        {
            if (playerLookDirection == 1f)
            {
                rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse); // 右に弾き飛ばす
            }
            else
            {
                rb.AddForce(Vector2.left * 15, ForceMode2D.Impulse); // 左に弾き飛ばす
            }
        }
        if (GameManager.game != null && GameManager.game.knockback_Player != null)
        {
            GameManager.game.knockback_Player.PlayerDodge();
        }
    }

    void VelocityDeceleration()
    {
        rb.velocity = new Vector2(rb.velocity.x * 0.7f, rb.velocity.y);
    }

    // 0.6秒後にisMove変数をfalseにする
    async UniTask SetIsMoveFalseAfterDelay()
    {
        // Enemyタグの付いたオブジェクトのCollider2Dを取得
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyColliders.Add(enemyCollider);
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        VelocityDeceleration();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        VelocityDeceleration();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        VelocityDeceleration();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        VelocityDeceleration();
        await UniTask.Delay(TimeSpan.FromSeconds(0.2));

        staticAfterImageEffect2DPlayer.enabled = false; // 残像の非表示

        isMove = false;
        dodging = false;

        // 衝突を再度有効にする
        foreach (Collider2D enemyCollider in enemyColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
        }
        enemyColliders.Clear();
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

        }
        else
        { //水から出たら
            jumpPower = jumpPowerKeep;
            jumpWater = 1f;
            rb.gravityScale = 2f;
            moveSpeed = moveSpeedKeep;
        }
    }

    /// <summary>
    /// ノックバック時の行動停止
    /// </summary>
    /// <param name="amount"></param>
    public void MoveControl(bool amount)
    {
        isMove = amount;

        // 移動速度を0にする
        rb.velocity = new Vector2(0, rb.velocity.y);

        // 0.3秒待ってからisMoveをfalseに設定し移動開始
        DOVirtual.DelayedCall(0.3f, () => {
            isMove = false;
        });
    }

    void PlayerAnim()
    {
        if (isGrounded != jumping) // 地面に接地している場合のみ
        {
            // Sキーが押されているかどうかをチェック
            if (Input.GetKey(KeyCode.S))
            {
                // Sキーが押された状態でAキーまたはDキーが押されているかをチェック
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    moveSpeed = defaultMoveSpeed / 2;
                    anim.Play("Player_Hofuku");
                }
                else
                {
                    // Sキーのみが押されている時
                    anim.Play("Player_FirstHofuku");
                }
            }
            else if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)))
                {
                // AキーまたはDキーが押され、シフトキーが押されて地面に足を着けている場合
                moveSpeed = defaultMoveSpeed * 1.8f;

                anim.Play("Player_Dash");
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                // AキーまたはDキーのみが押されている場合
                moveSpeed = defaultMoveSpeed;

                anim.Play("Player_Walk");
            }
            else
            {
                // 何もキーが押されていない場合
                anim.Play("Player_Idle");
            }
        }
        if (!isGrounded) 
        {
            //moveSpeed = defaultMoveSpeed;
        }
    }

    // ジャンプが終わったらアニメーションイベントから呼び出される関数
    public void OnJumpAnimationEnd()
    {
        anim.Play("Player_Fall");
    }

    // 主人公の向き
    void Player_Direction()
    {
        if (Input.GetKey(KeyCode.D) && Time.timeScale != 0)
        {
            this.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 左向き
        }
        if (Input.GetKey(KeyCode.A) && Time.timeScale != 0)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 右向き

        }
    }
}
