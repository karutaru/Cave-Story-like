using System.Collections;
using UnityEngine;

public class DelayedAnimation : MonoBehaviour
{
    // Animator�R���|�[�l���g�ւ̎Q��
    private Animator animator;

    // ����������
    void Start()
    {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();

        // �R���[�`�����J�n
        StartCoroutine(PlayMashAnimationWithDelay(2.5f));
    }

    // �w�肵���x�����Ԍ�ɃA�j���[�V�������Đ�����R���[�`��
    private IEnumerator PlayMashAnimationWithDelay(float delay)
    {
        // �w�肵�����Ԃ����ҋ@
        yield return new WaitForSeconds(delay);

        // "Mash"�A�j���[�V�������Đ�
        animator.Play("Mash");
    }
}
