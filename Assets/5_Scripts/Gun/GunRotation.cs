using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Camera mainCamera; // シーン内のカメラ

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
            // マウスのスクリーン座標をワールド座標に変換
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));

            // マウスの方向を計算
            Vector2 direction = -(mousePosition - transform.position).normalized;

            // その方向に武器を回転させる
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        //Vector3 angles = EulerAngles; // ﾌﾟﾛﾊﾟﾃｨの使い方
        //EulerAngles = Vector3.zero;
    }
}
