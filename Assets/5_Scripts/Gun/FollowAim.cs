using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAim : MonoBehaviour
{
    public Camera mainCamera;
    public float followSpeed = 0.1f; // �Ǐ]�̑��x�i0����1�̊Ԃ̒l�j

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // ���`��Ԃ��g�p���Ċ��炩�Ɉړ�
            transform.position = Vector3.Lerp(transform.position, mousePosition, followSpeed);
        }
    }
}
