using UnityEngine;

public class LockRotation : MonoBehaviour
{
    // �����̃��[�J����]��ێ����邽�߂̕ϐ�
    private Quaternion initialLocalRotation;

    void Start()
    {
        // �����̃��[�J����]��ۑ�
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        // �e�I�u�W�F�N�g�̉�]���t��]�����āA�����̃��[�J����]���ێ�
        transform.localRotation = initialLocalRotation * Quaternion.Inverse(transform.parent.rotation);
    }
}