using UnityEngine;

public class CameraPointController : MonoBehaviour
{
    public Transform player; // Player�I�u�W�F�N�g��Transform
    public Transform cameraPoint; // CameraPoint�I�u�W�F�N�g��Transform
    public float distanceAhead = 2.0f; // Player�̑O���ɐi�ދ���
    public float mouseSensitivity = 0.1f; // �}�E�X�̊��x

    private Vector3 initialOffset; // �����I�t�Z�b�g

    void Start()
    {
        // CameraPoint�̏����I�t�Z�b�g��ۑ�
        initialOffset = cameraPoint.position - player.position;
    }

    void Update()
    {
        // Player�̑O���ɐi�ވʒu���v�Z
        Vector3 forwardPosition = player.position + player.forward * distanceAhead;

        // �}�E�X�̈ʒu���擾
        Vector3 mousePosition = Input.mousePosition;

        // �}�E�X����ʓ��ɂ��邩�ǂ������`�F�b�N
        if (mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height)
        {
            mousePosition.z = Camera.main.transform.position.y; // �J�����̍�����Z���ɐݒ�
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // CameraPoint�̐V�����ʒu���v�Z
            Vector3 newPosition = forwardPosition + (worldMousePosition - player.position) * mouseSensitivity;

            // CameraPoint�̈ʒu���X�V
            cameraPoint.position = newPosition;
        }
    }
}