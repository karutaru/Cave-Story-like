using UnityEngine;
using Cinemachine;

//[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMouseOffset : MonoBehaviour
{
    public float sensitivity = 0.1f; // ずれの感度

    public CinemachineVirtualCamera cinemachineCamera;
    public CinemachineTransposer transposer;

    private void Start()
    {
        //cinemachineCamera = GetComponent<CinemachineVirtualCamera>();

        // CinemachineTransposer コンポーネントを取得
        transposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if (transposer == null) return;

        // ウィンドウの中心からのマウスの位置を取得
        Vector2 mouseOffset = new Vector2(
            (Input.mousePosition.x / Screen.width) - 0.5f,
            (Input.mousePosition.y / Screen.height) - 0.5f
        );

        // オフセットを適用
        Vector3 currentOffset = transposer.m_FollowOffset;
        currentOffset.x += mouseOffset.x * sensitivity;
        currentOffset.y += mouseOffset.y * sensitivity;

        transposer.m_FollowOffset = currentOffset;
    }
}
