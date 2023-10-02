using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Camera mainCamera; // �V�[�����̃J����

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
        }
    }
}
