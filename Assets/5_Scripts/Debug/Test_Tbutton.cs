using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Tbutton : MonoBehaviour
{
    // Animator�R���|�[�l���g���Q�Ƃ��邽�߂̕ϐ�
    private Animator anim;
    // ���݂̃A�j���[�V������Ԃ�ǐՂ��邽�߂̃t���O
    private bool isPlayingForward = true;

    void Start()
    {
        // Animator�R���|�[�l���g���擾
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // T�{�^���������ꂽ�Ƃ��̏���
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isPlayingForward)
            {
                // "Seed"�A�j���[�V�������Đ�
                anim.Play("Seed");
            }
            else
            {
                // "Seed_Reverse"�A�j���[�V�������Đ�
                anim.Play("Seed_Reverse");
            }
            // �t���O�𔽓]������
            isPlayingForward = !isPlayingForward;
        }
    }
}
