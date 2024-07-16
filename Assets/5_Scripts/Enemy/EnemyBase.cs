using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

public class EnemyBase : MonoBehaviour
{
    [Title("�X�e�[�^�X")]
    [HorizontalGroup("Row 1", Width = 0.5f)]
    [LabelText("�ő�HP"), HideLabel, LabelWidth(100)]
    public int hp = 5;

    [Title("")]
    [HorizontalGroup("Row 1")]
    [LabelText("�ړ����x"), HideLabel, LabelWidth(100)]
    public float speed = 40f;

    [HorizontalGroup("Row 2", Width = 0.5f)]
    [LabelText("�U����"), HideLabel, LabelWidth(100)]
    public int attack = 2;

    [HorizontalGroup("Row 2")]
    [LabelText("�W�����v��"), HideLabel, LabelWidth(100)]
    public float jumpPower = 20f;

    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [HorizontalGroup("Row 3", Width = 0.5f)]
    [LabelText("���Ƃ��o���l"), HideLabel, LabelWidth(100)]
    public int exp = 1;                         // �o���l

    [Title("�o�H�T��")]
    [HorizontalGroup("Row 4", Width = 0.5f)]
    [LabelText("���G����"), HideLabel, LabelWidth(100)]
    public float activateDistance = 50f;        // ���G����

    [Title("")]
    [HorizontalGroup("Row 4")]
    [LabelText("���G�p�X�X�V����"), HideLabel, LabelWidth(100)]
    public float pathUpdateSeconds = 0.5f;      //�p�X�̍X�V����

    [Title("�p�X����")]
    [HorizontalGroup("Row 5", Width = 0.5f)]
    [LabelText("���̳���߲�Ă܂ł̋���"), HideLabel, LabelWidth(150)]
    public float nextWayPointDistance = 0.5f;

    [Title("")]
    [HorizontalGroup("Row 5")]
    [LabelText("�W�����v���K�v�ȃm�[�h�̍���"), HideLabel, LabelWidth(150)]
    public float jumpNodeHeightRequirement = 0.8f;

    [Title("�s��")]
    [HorizontalGroup("Row 6", Width = 0.15f)]
    [LabelText("�ǐ�"), HideLabel, LabelWidth(30)]
    public bool followEnabled = true;           // �ǐՂ��\

    [Title("")]
    [HorizontalGroup("Row 6", Width = 0.15f)]
    [LabelText("�W�����v"), HideLabel, LabelWidth(40)]
    public bool jumpEnabled = true;             // �W�����v���\

    [Title("")]
    [HorizontalGroup("Row 6", Width = 0.15f)]
    [LabelText("�����ω�"), HideLabel, LabelWidth(50)]
    public bool directionLookEnabled = true;    // �����ω����\

    [Title("")]
    [HorizontalGroup("Row 6", Width = 0.13f)]
    [LabelText("���s"), HideLabel, LabelWidth(30)]
    public bool walkEnabled = true;             // ���s���\

    [Title("")]
    [HorizontalGroup("Row 6", Width = 0.13f)]
    [LabelText("����"), HideLabel, LabelWidth(30)]
    public bool stepSound = false;              // ���s�T�E���h��炷

    [Title("")]
    [HorizontalGroup("Row 6", Width = 0.13f)]
    [LabelText("�U��"), HideLabel, LabelWidth(30)]
    public bool attackEnabled = false;          // �U�����\

    [Title("")]
    [HorizontalGroup("Row 6", Width = 0.13f)]
    [LabelText("��s"), HideLabel, LabelWidth(30)]
    public bool flyEnabled = false;             // ��s���\

    [Header("�� �֘A")]
    public AudioClip walkSE;                    // ����SE
    public AudioClip idleSE;                    // �����~�܂�SE
    public float walkSoundCounter;              // �����̖�Ԋu

    //--------------------------�V�X�e��-------------------------------
    private bool showGroup;                     // �g�O���p�̕ϐ�

    [HideIfGroup("@showGroup == false")]
    [FoldoutGroup("showGroup")]
    [Header("�V�X�e��")]
    public LayerMask groundLayer;               // �ڒn����p�̃��C���[

    [FoldoutGroup("showGroup")]
    [Header("���ϐ��m�F�p")]
    public int currentHP;
    [FoldoutGroup("showGroup")]// ���݂�HP
    public bool canAttack = false;              // �U�����\��
    [FoldoutGroup("showGroup")]
    public Transform target;

    //-----------------------------�G��----------------------------------

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
    protected bool isGrounded = false; // �ڒn����
    protected Vector2 direction;
    protected BulletController bulletController;
    
    protected bool isShaking = false;

    protected int walkStateHash;
    protected int idleStateHash;
    protected int awakeStateHash;


    protected virtual void Start()
    {
        target = GameManager.game.playerObj;

        if (!TryGetComponent(out rb)) return;
        if (!TryGetComponent(out anim)) return;
        if (!TryGetComponent(out seeker)) return;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //���݂�HP���ő�l�܂ŉ�
        currentHP = hp;

        // �A�j���[�V�����̃n�b�V�����擾
        walkStateHash = Animator.StringToHash("Walk");
        idleStateHash = Animator.StringToHash("Idle");
        awakeStateHash = Animator.StringToHash("Awake");
    }

    protected virtual void Update()
    {
        // �W�����v�̔���
        isjumpTimer();

        // ���S����
        OrDeath();
    }

    protected virtual void FixedUpdate()
    {
        // �^�[�Q�b�g�Ƃ̋������߂��A�ǐՂł���ꍇ
        if (InDistanceTarget() && followEnabled)
        {
            // �ړ��挈��
            FollowPath();

            // �ړ��J�n
            StartMove();
        }

        else
        {
            // �ړ���~
            StopMove();
        }

        // ���sSE������A�����Ă��鎞
        if (stepSound && canStep)
        {
            // ����SE��炷
            IsWalkSound();
        }
    }

    //---------------------------------------------------------------��-�p�X�֘A-��-----------------------------------------------------------------------

    /// <summary>
    /// �p�X�̍X�V
    /// </summary>
    protected virtual void UpdatePath()
    {
        if (followEnabled && InDistanceTarget() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// �ǐՉ\
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

        // Direction�v�Z��
        PathDirection();

        // �W�����v��
        Jump();

        // �ړ���
        Move();

        // ��s��
        Fly();

        // �U����
        Attack();

        // ����Waypoint�̌v�Z��
        NextWayPointDistance();

        // ���Ă�������̏�����
        LookDirection();
    }

    //-------------------------------------------------------------------��-�ړ��֘A-��-------------------------------------------------------------------------


    /// <summary>
    /// �ړ�
    /// </summary>
    protected virtual void Move()
    {
        // �q���̃N���X�ŏ㏑�����Ďg��
    }

    /// <summary>
    /// �����o��
    /// </summary>
    protected virtual void StartMove()
    {
        moveTimer = 0;
    }

    /// <summary>
    /// �ړ���~
    /// </summary>
    protected virtual void StopMove()
    {
        if (walkEnabled && anim != null && moveTimer == 0)
        {
            // �~�܂�A�j���[�V����
            anim.Play("Idle");

            canStep = false;

        }
        if (idleSE != null && moveTimer == 0)
        {
            // �~�܂�SE���Đ�
            AudioSource.PlayClipAtPoint(idleSE, transform.position);

            canStep = false;
            moveTimer = 0;
        }
        moveTimer++;
    }


    /// <summary>
    /// �W�����v
    /// </summary>
    protected virtual void Jump()
    {
        // �ڒn����
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.5f, Color.red, 0.5f);

        // �W�����v����
        if (jumpEnabled && isGrounded && jumping == false)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                jumping = true;

                // �W�����v���s
                rb.AddForce(transform.up * jumpPower * 10f);
            }
        }
    }

    /// <summary>
    /// ����
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
    /// �W�����v���Ă��邩
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
    /// ��s
    /// </summary>
    protected virtual void Fly()
    {
        
    }

    /// <summary>
    /// ���Ă�������̏���
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

    //--------------------------------------------------------------��-�ŗL�̍s��-��-----------------------------------------------------------------

    /// <summary>
    /// �U���֘A
    /// </summary>
    protected virtual void Attack()
    {

    }

    //-------------------------------------------------------------�X�e�[�^�X�֘A------------------------------------------------------------------

    // �v���C���[�̒e���Ԃ�������
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //if (bulletController == col.GetComponent<BulletController>())
        if (col.TryGetComponent(out BulletController bulletController))
        {
            // �_���[�W���󂯂����ɍ��G�������ő�ɂ���
            activateDistance = 150f;

            // �v���C���[�̒e����_���[�W�������Ă���
            damage = bulletController.WeaponDamage;

            // �_���[�W���|�b�v�A�b�v
            GameObject damageTextObject = Instantiate(DataBase.instance.damagePrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            damageTextObject.GetComponent<TMP_Text>().text = damage.ToString();

            // �v���C���[�̒e�����������ʒu�Ƃ̍������v�Z
            Vector2 hitDiff = transform.position - bulletController.transform.position;

            if (damageTextObject.TryGetComponent(out Rigidbody2D rb))
            {
                int randomValue = Random.Range(30, 40);
                int randomUpValue = Random.Range(3, 8);
                rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                rb.AddForce(hitDiff.normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
            }




            // ���݂�HP����_���[�W��������
            currentHP -= damage;
        }

        // EffectDamage�R���|�[�l���g�����I�u�W�F�N�g�Ƃ̏Փ˂��`�F�b�N
        if (col.TryGetComponent(out EffectDamage effectDamage))
        {
            // �_���[�W���󂯂����ɍ��G�������ő�ɂ���
            activateDistance = 150f;

            // �G�t�F�N�g����_���[�W���擾
            int damage = effectDamage.effect_Damage;

            // �_���[�W���|�b�v�A�b�v
            GameObject damageTextObject = Instantiate(DataBase.instance.damagePrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            damageTextObject.GetComponent<TMP_Text>().text = damage.ToString();

            // �G�t�F�N�g�����������ʒu�Ƃ̍������v�Z
            Vector2 hitDiff = transform.position - col.transform.position;

            if (damageTextObject.TryGetComponent(out Rigidbody2D rb))
            {
                int randomValue = Random.Range(30, 40);
                int randomUpValue = Random.Range(3, 8);
                rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                rb.AddForce(hitDiff.normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
            }

            // ���݂�HP����_���[�W��������
            currentHP -= damage;
        }

        // �I�u�W�F�N�g��6���������E��1�h�b�g�h�炷����
        if (!isShaking)
        {
            StartCoroutine(ShakeObject());
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g��6���������E��1�h�b�g�h�炷
    /// </summary>
    protected IEnumerator ShakeObject()
    {
        isShaking = true;

        // 1�h�b�g = 1/100���j�b�g
        float shakeAmount = 0.09f;
        int shakeLoops = 6;

        for (int i = 0; i < shakeLoops; i++)
        {
            transform.position += new Vector3(shakeAmount, 0, 0);
            yield return new WaitForSeconds(0.04f);
            transform.position -= new Vector3(shakeAmount, 0, 0);
            yield return new WaitForSeconds(0.04f);
        }
        isShaking = false;
    }


    protected virtual void OrDeath()
    {
        // ���݂�HP��0�ȉ��ɂȂ�����
        if (currentHP <= 0)
        {
            // ���S����SE�Đ�
            AudioSource.PlayClipAtPoint(DataBase.instance.deathSE, transform.position);

            // �ŏ��̑傫�Ȕ����G�t�F�N�g�𐶐�
            GameObject ExplosionFirstEffect = Instantiate(DataBase.instance.ExplosionFirstPrefab, transform.position, Quaternion.identity);
            // ���܂��Ȕ����G�t�F�N�g�𐶐�
            GameObject ExplosionEffect = Instantiate(DataBase.instance.ExplosionPrefab, transform.position, Quaternion.identity);

            // ���������G�t�F�N�g��j��
            Destroy(ExplosionFirstEffect, 0.4f);
            Destroy(ExplosionEffect, 1f);

            // Exp�𐶐�
            ExpScript expScript;

            GameObject expObject = Instantiate(DataBase.instance.expPrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

            // Exp�Ɍo���l�̏��𑗂�
            expScript = expObject.GetComponent<ExpScript>();
            expScript.expValue = exp;

            // Exp�𐶐�
            GameObject expObject_2 = Instantiate(DataBase.instance.expPrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

            // Exp�Ɍo���l�̏��𑗂�
            expScript = expObject_2.GetComponent<ExpScript>();
            expScript.expValue = exp;


            // ���̃I�u�W�F�N�g��j��
            Destroy(this.gameObject);
        }
    }

    //----------------------------------------------------------------��-�p�X�֘A-��-----------------------------------------------------------------

    /// <summary>
    /// �ǐՑΏۂƂ̋������߂����true��Ԃ�
    /// </summary>
    /// <returns></returns>
    protected virtual bool InDistanceTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    /// <summary>
    /// �p�X����
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
    /// �p�X�̍���
    /// </summary>
    protected virtual void PathDirection()
    {
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// ����WayPoint�̋���
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

    //----------------------------------------------------------��-���̑�-��----------------------------------------------------------------------

    // DOTween���~�߂�
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        this.transform.DOKill();
    }


}
