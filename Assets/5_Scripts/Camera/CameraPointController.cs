using UnityEngine;

public class CameraPointController : MonoBehaviour
{
    public Transform player; // PlayerオブジェクトのTransform
    public Transform cameraPoint; // CameraPointオブジェクトのTransform
    public float distanceAhead = 2.0f; // Playerの前方に進む距離
    public float mouseSensitivity = 0.1f; // マウスの感度

    private Vector3 initialOffset; // 初期オフセット

    void Start()
    {
        // CameraPointの初期オフセットを保存
        initialOffset = cameraPoint.position - player.position;
    }

    void Update()
    {
        // Playerの前方に進む位置を計算
        Vector3 forwardPosition = player.position + player.forward * distanceAhead;

        // マウスの位置を取得
        Vector3 mousePosition = Input.mousePosition;

        // マウスが画面内にあるかどうかをチェック
        if (mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height)
        {
            mousePosition.z = Camera.main.transform.position.y; // カメラの高さをZ軸に設定
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // CameraPointの新しい位置を計算
            Vector3 newPosition = forwardPosition + (worldMousePosition - player.position) * mouseSensitivity;

            // CameraPointの位置を更新
            cameraPoint.position = newPosition;
        }
    }
}