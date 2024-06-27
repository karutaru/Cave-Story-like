using System.Collections;
using UnityEngine;

public class DelayedAnimation : MonoBehaviour
{
    // Animatorコンポーネントへの参照
    private Animator animator;

    // 初期化処理
    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        // コルーチンを開始
        StartCoroutine(PlayMashAnimationWithDelay(2.5f));
    }

    // 指定した遅延時間後にアニメーションを再生するコルーチン
    private IEnumerator PlayMashAnimationWithDelay(float delay)
    {
        // 指定した時間だけ待機
        yield return new WaitForSeconds(delay);

        // "Mash"アニメーションを再生
        animator.Play("Mash");
    }
}
