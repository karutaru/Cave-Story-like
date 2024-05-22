using UnityEngine;
using System.Collections; // コルーチンを使用するために必要

public class MecaDoorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // タグでプレイヤーを識別
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
    //    // アニメーションクリップの長さを待つ
    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    //    // 指定したアニメーションが終了したら、次のアニメーションを再生
    //    animator.Play(animationToStay);
    //}
}