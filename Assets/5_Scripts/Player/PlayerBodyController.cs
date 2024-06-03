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
    // �W�����v�Ɋւ���p�����[�^
    [SerializeField, Min(0)]
    private float jumpPower = 5f;               //�W�����v��
    private float jumpPowerKeep;
    private float jumpWater = 1f;               //�����̃W�����v��
    [SerializeField]
    AnimationCurve jumpCurve = new();
    [SerializeField]
    AnimationCurve jumpDownCurve = new();
    [SerializeField, Min(0)]
    private float maxJumpTime = 1f;             //����̌��E
    [SerializeField]
    private string JumpButtonName = "Jump";
    public bool isJump = true;                  // �W�����v�ł��邩
    private bool jumping = false;               //���ł��邩
    private bool jumpingKeep = false;           //���Ō�̋󒆂ɂ����true
    private bool isRun;                         //�����Ă���
    private Transform thisTransform;
    private float jumpTime = 0;
    private float t;
    private float power;
    private float gravity;
    public bool isMove;
    private float playerDash; // �_�b�V�����x


    private Rigidbody2D rb;                         // �v���C���[��Rigidbody2D
    private SpriteRenderer spriteRenderer;
    private string horizontal = "Horizontal";       // �L�[���͗p�̕�����w��(InputManager �� Horizontal �̓��͂𔻒肷�邽�߂̕�����)
    private GameObject headHitEffect;
    private bool headHit;
    private bool headHitOnce = false;               //����ł̂̓W�����v���P�񂾂�
    private bool jumpLandingOnce = false;           //���n����̂̓W�����v��P�񂾂�
    public bool inWater;                            //���̒���
    public bool isGrounded;
    private float rotation = 0;                     // �����̐ݒ�ɗ��p����
    public float playerLookDirection = -1f;         // ���̃v���C���[�̌���
    private float stepTimer;                        // �����v�̃J�E���g�p
    public float defaultMoveSpeed = 3.7f;           // �v���C���[�̈ړ����x
    private float moveSpeed;                         // �ړ����x�̉������ɗp���鐔�l
    private float moveSpeedKeep;                    // �ړ����x�̕ۑ�
    public float knockbackPower;                    // �G�ƐڐG�����ۂɐ�����΂�����
    public GameObject headHitEffectPrefab;
    private bool wasGroundedLastFrame = false;      // �O�̃t���[���ł̐ڒn��Ԃ��L�^
    private bool dodging;                           // ��𒆂�

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

    [SerializeField, Header("Linecast�p �n�ʔ��背�C���[")]
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
        if (isGrounded == true) //�ڒn���Ă�����
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
        else //�ڒn���Ă��Ȃ�������
        {
            if (inWater == false)
            {
                rb.gravityScale = 2f;
                jumpLandingOnce = true;
            }
        }

        //�ڒn����p�̃��C���L���X�g
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.4f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.4f, transform.position - transform.up * 0.6f, Color.red, 0.3f);

        //���ł�����p�̃��C���L���X�g
        headHit = Physics2D.Linecast(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.4f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.4f, Color.red, 0.3f);

        // �n�ʂɕt�����u�ԂɃA�C�h���A�j���[�V�������Đ�
        if (isGrounded && !wasGroundedLastFrame)
        {
            anim.Play("Player_Idle");
        }

        // ���݂̐ڒn��Ԃ����̃t���[���̂��߂ɋL�^
        wasGroundedLastFrame = isGrounded;

        if (headHit == true) //�V��ɓ���ł�����
        {
            if (headHitOnce == false)
            {
                //�G�t�F�N�g����
                headHitEffect = Instantiate(headHitEffectPrefab, new Vector2(this.transform.position.x, this.transform.position.y + 0.3f), this.transform.rotation);
                //���ł�SE�Đ�
                AudioSource.PlayClipAtPoint(headHitSE, transform.position);
                headHitOnce = true;
            }
            headHit = false;
            jumping = false;
            jumpTime = 0;
            //���ɔ��
            rb.AddForce(power * -Vector2.up, (ForceMode2D)ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(1) && !isMove)
        {
            isMove = true; // �ړ��֎~
            staticAfterImageEffect2DPlayer.enabled = true; // �c���̕\��
            SetIsMoveFalseAfterDelay().Forget();
            Dodge();
        }

        // �W�����v�̊J�n����
        if (isGrounded && Input.GetButtonDown(JumpButtonName) && isJump) //�ڒn���Ă��āA���W�����v�{�^�������������ɃW�����v��������Ă����ꍇ
        {
            if (Time.timeScale == 1)
            {
                anim.Play("Player_Jump");

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //S��������������
                {

                }
                else
                {
                    //anim.Play("Player_Jump");
                }
                //�W�����vSE�Đ�
                AudioSource.PlayClipAtPoint(jumpSE, transform.position);
                //���ł���
                jumping = true;
                //�����Ԃ�����
                headHitOnce = false;
            }
        }

        // �W�����v���I�����Ay���̑��x�����̒l�ɂȂ����ꍇ�ɗ����A�j���[�V�������Đ�
        if (!jumpingKeep && rb.velocity.y < 0)
        {
            anim.Play("Player_Fall");
        }

        if (!jumping && !dodging)
        {
            PlayerAnim();
        }


        if (jumping)                                             // �W�����v���̏�������
        {
            if (Input.GetButtonUp(JumpButtonName) || jumpTime >= maxJumpTime) //�W�����v�{�^���𗣂�����
            {
                jumping = false;
                rb.AddForce(-Vector2.up / (jumpTime + 0.1f) / jumpWater, (ForceMode2D)ForceMode.Impulse);
                jumpTime = 0;
            }
            else if (Input.GetButton(JumpButtonName)) //�W�����v�{�^�������������Ă����
            {
                jumpingKeep = true;
                jumpTime += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //����L�[�A��������W�L�[�������Ă���
        {
            if (Time.timeScale == 1)
            {
                if (isRun == true) //�����Ă���Œ��Ȃ�
                {

                }
                else
                { //�����Ă��Ȃ��Ȃ�

                }
            }
        }
        else
        { //��L�[�������Ă��Ȃ����

        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) //����L�[�A��������W�L�[�𗣂�����
        {
            if (jumping == true) //���ł�
            {

            }
            else
            { //���łȂ�

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
            if (jumping == true) //���ł�
            {

            }
            else
            { //���łȂ�

            }
        }
    }

    void FixedUpdate()
    {
        // �ړ�
        Move();
        // �W�����v
        Jump();
        // ��l���̌���
        Player_Direction();

    }

    /// <summary>
    /// �ړ�
    /// </summary>
    private void Move()                                           //�ړ�����
    {
        if (isMove)
        {
            return;
        }
        // ���������ւ̓��͎�t
        float x = Input.GetAxis(horizontal);

        if (x != 0)
        {
            //�����Ă��鎞
            isRun = true;
            //�����Ă���ԉ��Z
            stepTimer += Time.deltaTime;

            if (stepTimer >= 0.3f && isGrounded == true && Input.GetKey(KeyCode.LeftShift)) //��苗�������A���ڒn���Ă���L�V�t�g�������Ă��鎞
            {
                //����SE�Đ�
                AudioSource.PlayClipAtPoint(stepSE, transform.position);

                stepTimer = 0;
            }

            if (stepTimer >= 0.47f && isGrounded == true) //��苗�������A���ڒn���Ă��鎞
            {
                //����SE�Đ�
                AudioSource.PlayClipAtPoint(stepSE, transform.position);

                stepTimer = 0;
            }

            float targetVelocityX = x * moveSpeed;


            // ���݂̑��x��ڕW���x�Ɍ����ď��X�ɕύX
            float newVelocityX = Mathf.MoveTowards(rb.velocity.x, targetVelocityX, 20 * Time.deltaTime);

            // Rigidbody�̑��x���X�V
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);


            // temp �ϐ��Ɍ��݂� Rotation �l����
            Vector3 temp = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
            // ���݂̃L�[���͒l x �� temp.x �ɑ��
            temp.x = x;

            //�����l�ɂ���
            if (temp.x > 0)
            {
                playerLookDirection = 1f;
            }
            else
            {
                playerLookDirection = -1f;
            }

            if (isGrounded == true) //���łȂ�
            {
                
            }
            if (isGrounded == false)
            {

            }

        }
        else
        {
            //�����Ă��Ȃ���
            isRun = false;
        }
    }


    void Jump()                                             //�W�����v����
    {
        if (Time.timeScale == 1)
        {
            if (!jumping)
            {
                return;
            }

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

            //�W�����v�̑��x���A�j���[�V�����J�[�u����擾
            float t = jumpTime / maxJumpTime;
            float power = jumpPower * jumpCurve.Evaluate(t);

            if (t >= 1.0) //�W�����v���Ԃ̌��E�ȏ�Ȃ�
            {
                //�W�����v���~�߂�
                jumping = false;
                jumpTime = 0;
            }

            //��ɔ��
            rb.AddForce(power * Vector2.up, (ForceMode2D)ForceMode.Impulse);
        }
    }

    void Dodge()
    {
        dodging = true;
        anim.Play("Player_Dodge");
        rb.velocity = new Vector2(0, rb.velocity.y); // x�̈ړ����x����u0�Ƀ��Z�b�g

        if (Input.GetKey(KeyCode.A)) // ���L�[�������Ă���ꍇ
        {
            rb.AddForce(Vector2.left * 15, ForceMode2D.Impulse); // ���ɒe����΂�
        }
        else if (Input.GetKey(KeyCode.D)) // �E�L�[�������Ă���ꍇ
        {
            rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse); // �E�ɒe����΂�
        }
        else      // ���������Ă��Ȃ��ꍇ
        {
            if (playerLookDirection == 1f)
            {
                rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse); // �E�ɒe����΂�
            }
            else
            {
                rb.AddForce(Vector2.left * 15, ForceMode2D.Impulse); // ���ɒe����΂�
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

    // 0.6�b���isMove�ϐ���false�ɂ���
    async UniTask SetIsMoveFalseAfterDelay()
    {
        // Enemy�^�O�̕t�����I�u�W�F�N�g��Collider2D���擾
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

        staticAfterImageEffect2DPlayer.enabled = false; // �c���̔�\��

        isMove = false;
        dodging = false;

        // �Փ˂��ēx�L���ɂ���
        foreach (Collider2D enemyCollider in enemyColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
        }
        enemyColliders.Clear();
    }


    public void WaterMove(bool waterIn)                     //���̒�����
    {
        inWater = waterIn;
        if (inWater == true) //���ɓ�������
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.1f);
            //�W�����v�͂�ۑ�
            jumpPowerKeep = jumpPower;
            //�W�����v��2����1�ɂ���
            jumpPower = jumpPower / 2f;
            jumpWater = 1000f;
            rb.gravityScale = 1f;
            moveSpeedKeep = moveSpeed;
            moveSpeed = moveSpeed / 2;

        }
        else
        { //������o����
            jumpPower = jumpPowerKeep;
            jumpWater = 1f;
            rb.gravityScale = 2f;
            moveSpeed = moveSpeedKeep;
        }
    }

    /// <summary>
    /// �m�b�N�o�b�N���̍s����~
    /// </summary>
    /// <param name="amount"></param>
    public void MoveControl(bool amount)
    {
        isMove = amount;

        // �ړ����x��0�ɂ���
        rb.velocity = new Vector2(0, rb.velocity.y);

        // 0.3�b�҂��Ă���isMove��false�ɐݒ肵�ړ��J�n
        DOVirtual.DelayedCall(0.3f, () => {
            isMove = false;
        });
    }

    void PlayerAnim()
    {
        if (isGrounded != jumping) // �n�ʂɐڒn���Ă���ꍇ�̂�
        {
            // S�L�[��������Ă��邩�ǂ������`�F�b�N
            if (Input.GetKey(KeyCode.S))
            {
                // S�L�[�������ꂽ��Ԃ�A�L�[�܂���D�L�[��������Ă��邩���`�F�b�N
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    moveSpeed = defaultMoveSpeed / 2;
                    anim.Play("Player_Hofuku");
                }
                else
                {
                    // S�L�[�݂̂�������Ă��鎞
                    anim.Play("Player_FirstHofuku");
                }
            }
            else if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift)))
                {
                // A�L�[�܂���D�L�[��������A�V�t�g�L�[��������Ēn�ʂɑ��𒅂��Ă���ꍇ
                moveSpeed = defaultMoveSpeed * 1.8f;

                anim.Play("Player_Dash");
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                // A�L�[�܂���D�L�[�݂̂�������Ă���ꍇ
                moveSpeed = defaultMoveSpeed;

                anim.Play("Player_Walk");
            }
            else
            {
                // �����L�[��������Ă��Ȃ��ꍇ
                anim.Play("Player_Idle");
            }
        }
        if (!isGrounded) 
        {
            //moveSpeed = defaultMoveSpeed;
        }
    }

    // �W�����v���I�������A�j���[�V�����C�x���g����Ăяo�����֐�
    public void OnJumpAnimationEnd()
    {
        anim.Play("Player_Fall");
    }

    // ��l���̌���
    void Player_Direction()
    {
        if (Input.GetKey(KeyCode.D) && Time.timeScale != 0)
        {
            this.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // ������
        }
        if (Input.GetKey(KeyCode.A) && Time.timeScale != 0)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // �E����

        }
    }
}
