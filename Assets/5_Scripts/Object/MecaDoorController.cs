using UnityEngine;
using System.Collections; // �R���[�`�����g�p���邽�߂ɕK�v

public class MecaDoorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // �^�O�Ńv���C���[������
        {
            //StartCoroutine(PlayAnimationAndWait("MecaDoor_OpenAnime", "MecaDoor_Open"));
            animator.Play("MecaDoor_OpenAnime");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //StartCoroutine(PlayAnimationAndWait("MecaDoor_CloseAnime", "MecaDoor_Close"));
            animator.Play("MecaDoor_CloseAnime");
        }
    }

    //IEnumerator PlayAnimationAndWait(string animationToPlay, string animationToStay)
    //{
    //    animator.Play(animationToPlay);
    //    // �A�j���[�V�����N���b�v�̒�����҂�
    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    //    // �w�肵���A�j���[�V�������I��������A���̃A�j���[�V�������Đ�
    //    animator.Play(animationToStay);
    //}
}