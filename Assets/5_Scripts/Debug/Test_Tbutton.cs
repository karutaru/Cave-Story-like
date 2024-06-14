using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Tbutton : MonoBehaviour
{
    // Animatorコンポーネントを参照するための変数
    private Animator anim;
    // 現在のアニメーション状態を追跡するためのフラグ
    private bool isPlayingForward = true;

    void Start()
    {
        // Animatorコンポーネントを取得
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Tボタンが押されたときの処理
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isPlayingForward)
            {
                // "Seed"アニメーションを再生
                anim.Play("Seed");
            }
            else
            {
                // "Seed_Reverse"アニメーションを再生
                anim.Play("Seed_Reverse");
            }
            // フラグを反転させる
            isPlayingForward = !isPlayingForward;
        }
    }
}
