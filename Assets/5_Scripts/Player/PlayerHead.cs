using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    public Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    public Sprite upSprite; // ��̃X�v���C�g
    public Sprite downSprite; // ���̃X�v���C�g
    public Sprite normalSprite; // �ʏ펞�̃X�v���C�g
    public Sprite rightSprite; // �E�����̃X�v���C�g
    public Sprite upRightSprite; // ��ƉE�����̃X�v���C�g

    public bool look_L = true;



    private void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            CheckMouseAngle();
        }
    }

    void CheckMouseAngle() // �p�x���ƂɃv���C���[�̊���X�V
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;�@// �}�E�X�̕������v�Z
        float angle = Vector2.SignedAngle(transform.up, direction);
        float angle_down = Vector2.SignedAngle(-transform.up, direction);



        if (angle_down > 110 && angle_down <= 180 && look_L == true)
        {
            // �}�E�X�̈ʒu���v���C���[�̏�ƉE����70�x�ȓ��ɂ���ꍇ�̏���
            spriteRenderer.sprite = upRightSprite;
            return;
        }
        if (angle > 0 && angle <= 70 && look_L == false)
        {
            // �}�E�X�̈ʒu���v���C���[�̏�ƍ�����70�x�ȓ��ɂ���ꍇ�̏���
            spriteRenderer.sprite = upRightSprite;
            return;
        }
        if (Mathf.Abs(angle) <= 50)
        {
            // �}�E�X�̈ʒu���v���C���[�̏�90�x�ȓ��ɂ���ꍇ�̏���
            if (spriteRenderer && upSprite)
            {
                spriteRenderer.sprite = upSprite;
            }
            return;
        }
        if (Mathf.Abs(angle_down) <= 50)
        {
            // �}�E�X�̈ʒu���v���C���[�̉�90�x�ȓ��ɂ���ꍇ�̏���
            if (spriteRenderer && downSprite)
            {
                spriteRenderer.sprite = downSprite;
            }
            return;
        }
        if (angle_down > 40 && angle_down <= 130 && look_L == true)
        {
            // �}�E�X�̈ʒu���v���C���[�̉E����90�x�ȓ��ɂ���ꍇ�̏���
            spriteRenderer.sprite = rightSprite;

            return;
        }
        if (angle > 40 && angle <= 130 && look_L == false)
        {
            // �}�E�X�̈ʒu���v���C���[�̉E����90�x�ȓ��ɂ���ꍇ�ŁA���v���C���[�����]���Ă��鎞�̏���
            spriteRenderer.sprite = rightSprite;

            return;
        }
        if (spriteRenderer && normalSprite)
        {
            // �f�t�H���g�̊�
            spriteRenderer.sprite = normalSprite;

            return;
        }
    }

    public void LookL(bool amount)
    {
        look_L = amount;
    }
}
