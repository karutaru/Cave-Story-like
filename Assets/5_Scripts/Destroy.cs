using UnityEngine;

public class Destroy : MonoBehaviour
{
    // �A�j���[�V�����̒x�����ԁi�I�v�V�����j
    public float delay = 0.2f;

    void Start()
    {
        // Animator�R���|�[�l���g���擾
        Animator animator = GetComponent<Animator>();

        // ���݂̃A�j���[�V�����̒������擾
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // �A�j���[�V�����̒����ɒx�����Ԃ����Z���āA�I�u�W�F�N�g��j��
        Destroy(gameObject, animationLength + delay);
    }
}
