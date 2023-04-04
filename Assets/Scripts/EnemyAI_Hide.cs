using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using UnityEngine.UIElements;

/// <summary>
/// �G�l�~�[�̍s���Ɋւ���X�N���v�g
/// </summary>
public class EnemyAI_Hide : EnemyBase
{
    private int awakeCounter;
    private int waitCounter;
    private bool isAwake;

    //-----------------------------------------------------�����܂�---------------------------------------------------------------


    protected override void Start()
    {
        if (!TryGetComponent(out rb)) return;
        if (!TryGetComponent(out anim)) return;
        if (!TryGetComponent(out seeker)) return;


        walkEnabled = false;
        anim.Play("Idle");

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        //���݂�HP���ő�l�܂ŉ�
        currentHP = hp;
    }

    protected override void Update()
    {
        // �N��������
        IsAwake();

        // �W�����v�̔���
        isjumpTimer();

        // ���S����
        OrDeath();


    }

    protected override void FixedUpdate()
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

    //-------------------------------------------------------------------��-�ړ��֘A-��-------------------------------------------------------------------------

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
        //0.5�b��~
        yield return new WaitForSeconds(1f);

        walkEnabled = true;
        isAwake = true;


    }

    /// <summary>
    /// �ړ�
    /// </summary>
    protected override void Move()
    {
        //�ړ�
        if (walkEnabled && Vector2.Distance(transform.position, target.transform.position) < activateDistance)
        {
            if (anim != null)
            {
                // �����A�j���[�V�������Đ�
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
        //    // �����~�܂�A�j���[�V�������Đ�
        //    if (walkEnabled)
        //    {
        //        anim.Play("Idle");
        //    }
        //}
    }

    /// <summary>
    /// �����o��
    /// </summary>
    protected override void StartMove()
    {
        moveTimer = 0;
    }

    /// <summary>
    /// �ړ���~
    /// </summary>
    protected override void StopMove()
    {
        if (anim != null && moveTimer == 0)
        {
            // �~�܂�A�j���[�V����
            if (walkEnabled)
            {
                anim.Play("Idle");
            }

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
    protected override void Jump()
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
    /// �W�����v���Ă��邩
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

    //--------------------------------------------------------------��-�ŗL�̍s��-��-----------------------------------------------------------------

    /// <summary>
    /// �U���֘A
    /// </summary>
    protected override void Attack()
    {

    }

    //-------------------------------------------------------------�X�e�[�^�X�֘A------------------------------------------------------------------

    // �v���C���[�̒e���Ԃ�������
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (bulletController = col.GetComponent<BulletController>())
        {
            // �v���C���[�̒e����_���[�W�������Ă���
            damage = bulletController.weaponDamage;

            // ���݂�HP����_���[�W��������
            currentHP -= damage;
        }
    }

    //----------------------------------------------------------------��-�p�X�֘A-��-----------------------------------------------------------------



    //----------------------------------------------------------��-���̑�-��----------------------------------------------------------------------


}
