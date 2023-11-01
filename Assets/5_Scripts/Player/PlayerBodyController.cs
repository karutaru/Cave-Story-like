using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Sirenix.OdinInspector.Editor.Internal.FastDeepCopier;
using DG.Tweening;

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
    private bool jumping = false;               //���ł��邩
    private bool isRun;                         //�����Ă���
    private Transform thisTransform;
    private float jumpTime = 0;
    private float t;
    private float power;
    private float gravity;
    private bool isMove;

    [SerializeField]
    private Rigidbody2D rb;                         // �v���C���[��Rigidbody2D
    private string horizontal = "Horizontal";       // �L�[���͗p�̕�����w��(InputManager �� Horizontal �̓��͂𔻒肷�邽�߂̕�����)
    //private Animator anim;
    private GameObject headHitEffect;
    private bool headHit;
    private bool headHitOnce = false;               //����ł̂̓W�����v���P�񂾂�
    private bool jumpLandingOnce = false;           //���n����̂̓W�����v��P�񂾂�
    public bool inWater;                            //���̒���
    public bool isGrounded;
    private float rotation = 0;                     // �����̐ݒ�ɗ��p����
    public float playerLookDirection = -1f;         // ���̃v���C���[�̌���
    private float stepTimer;                        //�����v�̃J�E���g�p
    public float moveSpeed;                         //�ړ����x
    private float moveSpeedKeep;                    //�ړ����x�̕ۑ�
    public float knockbackPower;                    // �G�ƐڐG�����ۂɐ�����΂�����
    public GameObject headHitEffectPrefab;
    public AudioClip headHitSE;
    public AudioClip jumpLandingSE;
    public AudioClip jumpSE;
    public AudioClip stepSE;

    public Knockback_Player knockback_Player;

    [SerializeField]
    CinemachineVirtualCamera artViewCamera;
    CinemachineFramingTransposer artViewCameraTransposer;

    [SerializeField, Header("Linecast�p �n�ʔ��背�C���[")]
    private LayerMask groundLayer;


    void Start()
    {
        artViewCameraTransposer = artViewCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        rotation = transform.localRotation.x; //0
        thisTransform = transform;
        playerLookDirection = -1f;
    }

    void Update()
    {
        if (isGrounded == true) //�ڒn���Ă�����
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


        // �W�����v�̊J�n����
        if (isGrounded && Input.GetButtonDown(JumpButtonName)) //�ڒn���Ă��āA���W�����v�{�^������������
        {
            if (Time.timeScale == 1)
            {
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
                jumpTime += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //����L�[�A��������W�L�[�������Ă���
        {
            if (Time.timeScale == 1)
            {
                if (isRun == true) //�����Ă���Œ��Ȃ�
                {
                    //anim.Play("LookUp");
                    //anim.SetFloat("LookUpIdle", 0.0f);
                }
                else
                { //�����Ă��Ȃ��Ȃ�
                    //anim.Play("LookUpIdle");
                    //anim.SetFloat("LookUp", 0.0f);
                }
            }
        }
        else
        { //��L�[�������Ă��Ȃ����

            //anim.SetFloat("LookUpIdle", 0.0f);
            //anim.SetFloat("LookUp", 0.0f);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) //����L�[�A��������W�L�[�𗣂�����
        {
            if (jumping == true) //���ł�
            {
                //anim.SetFloat("LookUpIdle", 0.0f);
                //anim.SetFloat("LookUp", 0.0f);
                //anim.Play("Player_Jump");
            }
            else
            { //���łȂ�
                //anim.Play("Player_Idle");
            }
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.timeScale == 1)
            {
                //anim.Play("Player_LookDown");
            }
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (jumping == true) //���ł�
            {
                //anim.Play("Player_Jump");
            }
            else
            { //���łȂ�
                //anim.Play("Player_Idle");
            }
        }
    }

    void FixedUpdate()
    {
        //�ړ�
        Move();
        //�W�����v
        Jump();

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

            if (stepTimer >= 0.3f && isGrounded == true) //��苗�������A���ڒn���Ă��鎞
            {
                //����SE�Đ�
                AudioSource.PlayClipAtPoint(stepSE, transform.position);

                stepTimer = 0;
            }

            // x �̒l�� 0 �ł͂Ȃ��ꍇ = �L�[���͂�����ꍇ
            //rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);


            float targetVelocityX = x * moveSpeed;

            // ���݂̑��x��ڕW���x�Ɍ����ď��X�ɕύX
            float newVelocityX = Mathf.MoveTowards(rb.velocity.x, targetVelocityX, 10 * Time.deltaTime);

            // Rigidbody�̑��x���X�V
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);


            // temp �ϐ��Ɍ��݂� Rotation �l����
            Vector3 temp = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
            // ���݂̃L�[���͒l x �� temp.x �ɑ��
            temp.x = x;

            //�����l�ɂ���
            if (temp.x > 0)
            {
                //0�����傫�����1�ɂ���
                //this.transform.rotation = Quaternion.Euler(0f, 180f, 0f); //�E����
                playerLookDirection = 1f;
            }
            else
            {
                //0�������������-1�ɂ���
                //this.transform.rotation = Quaternion.Euler(0f, 0f, 0f); //������
                playerLookDirection = -1f;
            }
            //�L�����̌������ړ������ɍ��킹��
            // transform.localScale = temp;

            if (isGrounded == true) //���łȂ�
            {
                ////�ҋ@��Ԃ̃A�j���̍Đ����~�߂āA����A�j���̍Đ��ւ̑J�ڂ��s��
                //anim.SetFloat("Run", 0.2f);
            }
            if (isGrounded == false)
            {
                //anim.SetFloat("Run", 0.0f);
            }

        }
        else
        {
            //�����Ă��Ȃ���
            isRun = false;
            // ���E�̓��͂��Ȃ������牡�ړ��̑��x��0�ɂ��Ă����ɒ�~����
            //rb.velocity = new Vector2(0, rb.velocity.y);
            //// ����A�j���̍Đ����~�߂āA�ҋ@��Ԃ̃A�j���̍Đ��ւ̑J�ڂ��s��
            //anim.SetFloat("Run", 0.0f);
            // velocity(���x)�ɐV�����l�������Ĉړ�
            //rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


    void Jump()                                             //�W�����v����
    {
        if (Time.timeScale == 1)
        {
            if (!jumping) //���ł��Ȃ�
            {
                //�~�߂�
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


    //private void OnCollisionEnter2D(Collision2D col)
    //{ //�ڐG�����Ȃ�

    //    // �ڐG�����R���C�_�[�����Q�[���I�u�W�F�N�g��Tag��Enemy�Ȃ� 
    //    if (col.gameObject.tag == "Enemy")
    //    {

    //        // �L�����ƓG�̈ʒu���狗���ƕ������v�Z
    //        Vector3 direction = (transform.position - col.transform.position).normalized;

    //        // �G�̔��Α��ɃL�����𐁂���΂�
    //        transform.position += direction * knockbackPower;
    //    }
    //}

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
}
