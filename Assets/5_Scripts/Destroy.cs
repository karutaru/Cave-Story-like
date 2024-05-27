using UnityEngine;

public class Destroy : MonoBehaviour
{
    // アニメーションの遅延時間（オプション）
    public float delay = 0.2f;

    void Start()
    {
        // Animatorコンポーネントを取得
        Animator animator = GetComponent<Animator>();

        // 現在のアニメーションの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // アニメーションの長さに遅延時間を加算して、オブジェクトを破壊
        Destroy(gameObject, animationLength + delay);
    }
}
