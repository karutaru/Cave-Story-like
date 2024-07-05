using UnityEngine;
using Cinemachine;

public class ParallaxEffect : MonoBehaviour
{
    // Cinemachine�J�����̎Q��
    public CinemachineVirtualCamera virtualCamera;
    // �p�����b�N�X���ʂ̋���
    public Vector2 parallaxStrength;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        // �J������Transform���擾
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
        // �����J�����ʒu��ۑ�
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // �J�����̈ړ��ʂ��v�Z
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        // �w�i�̈ʒu�𒲐�
        transform.position += new Vector3(deltaMovement.x * parallaxStrength.x, deltaMovement.y * parallaxStrength.y, 0);
        // ���݂̃J�����ʒu��ۑ�
        previousCameraPosition = cameraTransform.position;
    }
}