using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public Transform cameraPoint; // CameraPointオブジェクトへの参照
    public float followSpeed = 5f; // CameraPointの移動速度
    public float maxDistance = 2f; // プレイヤーからの最大距離

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // マウスの位置をプレイヤーの位置に限定するためのオフセットを計算
        Vector3 offset = mousePosition - transform.position;
        offset.z = 0; // 2Dゲームなので、Z軸は無視

        // オフセットを制限する
        offset = Vector3.ClampMagnitude(offset, maxDistance);

        // CameraPointを目標位置に滑らかに移動
        cameraPoint.position = Vector3.Lerp(cameraPoint.position, transform.position + offset, followSpeed * Time.deltaTime);
    }
}