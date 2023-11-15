using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public Transform cameraPoint; // CameraPoint�I�u�W�F�N�g�ւ̎Q��
    public float followSpeed = 5f; // CameraPoint�̈ړ����x
    public float maxDistance = 2f; // �v���C���[����̍ő勗��

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // �}�E�X�̈ʒu���v���C���[�̈ʒu�Ɍ��肷�邽�߂̃I�t�Z�b�g���v�Z
        Vector3 offset = mousePosition - transform.position;
        offset.z = 0; // 2D�Q�[���Ȃ̂ŁAZ���͖���

        // �I�t�Z�b�g�𐧌�����
        offset = Vector3.ClampMagnitude(offset, maxDistance);

        // CameraPoint��ڕW�ʒu�Ɋ��炩�Ɉړ�
        cameraPoint.position = Vector3.Lerp(cameraPoint.position, transform.position + offset, followSpeed * Time.deltaTime);
    }
}