using UnityEngine;
using Cinemachine;

public class ParallaxEffect : MonoBehaviour
{
    // Cinemachineカメラの参照
    public CinemachineVirtualCamera virtualCamera;
    // パララックス効果の強さ
    public Vector2 parallaxStrength;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        // カメラのTransformを取得
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
        // 初期カメラ位置を保存
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // カメラの移動量を計算
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        // 背景の位置を調整
        transform.position += new Vector3(deltaMovement.x * parallaxStrength.x, deltaMovement.y * parallaxStrength.y, 0);
        // 現在のカメラ位置を保存
        previousCameraPosition = cameraTransform.position;
    }
}