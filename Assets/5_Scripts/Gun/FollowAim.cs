using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAim : MonoBehaviour
{
    public Camera mainCamera;
    public float followSpeed = 0.1f; // 追従の速度（0から1の間の値）

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

            // 線形補間を使用して滑らかに移動
            transform.position = Vector3.Lerp(transform.position, mousePosition, followSpeed);
        }
    }
}
