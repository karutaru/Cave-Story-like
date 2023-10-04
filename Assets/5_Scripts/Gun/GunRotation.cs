using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Camera mainCamera; // �V�[�����̃J����
    private SpriteRenderer spriteRenderer;

    private Sprite currentSprite;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector3 EulerAngles { 
        get { return transform.eulerAngles; }
        private set { transform.eulerAngles = value; }
    }

    public Vector3 EulerAngles2
    {
        get => transform.eulerAngles;
        private set => transform.eulerAngles = value;
    }

    public Quaternion EulerAngles3 => transform.rotation;

    void Update()
    {
        if (Time.timeScale == 1)
        {
            // �}�E�X�̃X�N���[�����W�����[���h���W�ɕϊ�
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));

            // �}�E�X�̕������v�Z
            Vector2 direction = -(mousePosition - transform.position).normalized;

            // ���̕����ɕ������]������
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);


            // �}�E�X�̃X�N���[�����W���X�N���[���̒��������E���ɂ��邩�𔻒f
            if (Input.mousePosition.x > Screen.width / 2)
            {
                // �E���ɂ���ꍇ�A�X�v���C�g��Y���𔽓]
                spriteRenderer.flipY = true;
            }
            else
            {
                // �����ɂ���ꍇ�A���]������
                spriteRenderer.flipY = false;
            }
        }

        //Vector3 angles = EulerAngles; // �����è�̎g����
        //EulerAngles = Vector3.zero;
    }
}
