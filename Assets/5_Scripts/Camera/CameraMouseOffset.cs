using UnityEngine;
using Cinemachine;

//[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMouseOffset : MonoBehaviour
{
    public float sensitivity = 0.1f; // ����̊��x

    public CinemachineVirtualCamera cinemachineCamera;
    public CinemachineTransposer transposer;

    private void Start()
    {
        //cinemachineCamera = GetComponent<CinemachineVirtualCamera>();

        // CinemachineTransposer �R���|�[�l���g���擾
        transposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if (transposer == null) return;

        // �E�B���h�E�̒��S����̃}�E�X�̈ʒu���擾
        Vector2 mouseOffset = new Vector2(
            (Input.mousePosition.x / Screen.width) - 0.5f,
            (Input.mousePosition.y / Screen.height) - 0.5f
        );

        // �I�t�Z�b�g��K�p
        Vector3 currentOffset = transposer.m_FollowOffset;
        currentOffset.x += mouseOffset.x * sensitivity;
        currentOffset.y += mouseOffset.y * sensitivity;

        transposer.m_FollowOffset = currentOffset;
    }
}
