using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    public int hp = 5;                          // �ő�HP
    public int attack = 2;                      // �U����
    public float speed = 40f;                   // ���̑���
    public float jumpPower = 20f;               // �W�����v��
    public int exp = 1;                         // �o���l

    [Header("�o�H�T��")]
    public Transform target;                    // �^�[�Q�b�g�̈ʒu
    public float activateDistance = 50f;        // ���G����
    public float pathUpdateSeconds = 0.5f;      //�p�X�̍X�V����

    [Header("�p�X����")]
    public float nextWayPointDistance = 0.5f;
    public float jumpNodeHeightRequirement = 0.8f;

    [Header("�s���̋���")]
    public bool followEnabled = true;           // �ǐՂ��\
    public bool jumpEnabled = true;             // �W�����v���\
    public bool directionLookEnabled = true;    // �����ω����\
    public bool walkEnabled = true;             // ���s���\
    public bool stepSound = false;              // ���s�T�E���h��炷
    public bool attackEnabled = false;          // �U�����\
    public bool flyEnabled = false;             // ��s���\
    [Header("�f�ނ̗L��")]
    public bool walkAnimEnable = false;         // �����A�j���̗L��
    public bool walkSoundEnable = false;        // �������̗L��
    public bool idleAnimEnable = false;         // �~�܂�A�j���̗L��
    public bool idleSoundEnable = false;        // �~�܂鉹�̗L��
    public bool attackSoundEnable = false;      // �U�����̗L��

    [Header("�� �֘A")]
    public AudioClip walkSE;                    // ����SE
    public AudioClip idleSE;                    // �����~�܂�SE
    public AudioClip deathSE;                   // ���S����SE
    public float walkSoundCounter;              // �����̖�Ԋu

    [Header("�v���n�u �֘A")]
    public GameObject expPrefabs;               // �o���l�̃v���n�u
    public GameObject damagePrefabs;            // �_���[�W�e�L�X�g�̃v���n�u
    public GameObject ExplosionFirstPrefab;     // �ŏ��̑傫�Ȕ����̃v���n�u
    public GameObject ExplosionPrefab;          // ���_�ׂ̍��Ȕ����̃v���n�u

    [Header("�V�X�e��")]
    public LayerMask groundLayer;               // �ڒn����p�̃��C���[

    //--------------------------���I�ȕϐ�-------------------------------

    [Header("���ϐ��m�F�p")]
    public int currentHP;                       // ���݂�HP
    public bool canAttack = false;              // �U�����\��

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


    protected virtual void Start()
    {
        if (!TryGetComponent(out rb)) return;
        if (!TryGetComponent(out anim)) return;
        if (!TryGetComponent(out seeker)) return;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //���݂�HP���ő�l�܂ŉ�
        currentHP = hp;
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
        if (walkEnabled && anim != null && moveTimer == 0 && idleAnimEnable)
        {
            // �~�܂�A�j���[�V����
            anim.Play("Idle");

            canStep = false;

        }
        if (idleSE != null && moveTimer == 0 && idleSoundEnable)
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
            // �v���C���[�̒e����_���[�W�������Ă���
            damage = bulletController.WeaponDamage;

            // �_���[�W���|�b�v�A�b�v
            GameObject damageTextObject = Instantiate(damagePrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            Debug.Log(damageTextObject.name);
            damageTextObject.GetComponent<TMP_Text>().text = damage.ToString();

            // �v���C���[�̒e�����������ʒu�Ƃ̍������v�Z
            Vector2 hitDiff = transform.position - bulletController.transform.position;
            // �e�L�X�g���v���C���[�̒e�����������ʒu�Ƃ͋t�����Ɍ������ă|�b�v�A�b�v����悤�ɁAAddForce�ŗ͂�������
            //Rigidbody2D textRb = damageTextObject.GetComponent<Rigidbody2D>();
            //textRb.AddForce(hitDiff.normalized * 50f, ForceMode2D.Impulse);

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
    }

    protected virtual void OrDeath()
    {
        // ���݂�HP��0�ȉ��ɂȂ�����
        if (currentHP <= 0)
        {
            // ���S����SE�Đ�
            AudioSource.PlayClipAtPoint(deathSE, transform.position);

            // �ŏ��̑傫�Ȕ����G�t�F�N�g�𐶐�
            GameObject ExplosionFirstEffect = Instantiate(ExplosionFirstPrefab, transform.position, Quaternion.identity);
            // ���܂��Ȕ����G�t�F�N�g�𐶐�
            GameObject ExplosionEffect = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

            // ���������G�t�F�N�g��j��
            Destroy(ExplosionFirstEffect, 0.4f);
            Destroy(ExplosionEffect, 1f);

            // Exp�𐶐�
            ExpScript expScript;

            GameObject expObject = Instantiate(expPrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

            // Exp�Ɍo���l�̏��𑗂�
            expScript = expObject.GetComponent<ExpScript>();
            expScript.expValue = exp;

            // Exp�𐶐�
            GameObject expObject_2 = Instantiate(expPrefabs, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

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
