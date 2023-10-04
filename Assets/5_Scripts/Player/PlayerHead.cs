using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    public Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    public Sprite upSprite; // 上のスプライト
    public Sprite downSprite; // 下のスプライト
    public Sprite normalSprite; // 通常時のスプライト
    public Sprite rightSprite; // 右向きのスプライト
    public Sprite upRightSprite; // 上と右向きのスプライト

    public bool look_L = true;



    private void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            CheckMouseAngle();
        }
    }

    void CheckMouseAngle() // 角度ごとにプレイヤーの顔を更新
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;　// マウスの方向を計算
        float angle = Vector2.SignedAngle(transform.up, direction);
        float angle_down = Vector2.SignedAngle(-transform.up, direction);



        if (angle_down > 110 && angle_down <= 180 && look_L == true)
        {
            // マウスの位置がプレイヤーの上と右側の70度以内にある場合の処理
            spriteRenderer.sprite = upRightSprite;
            return;
        }
        if (angle > 0 && angle <= 70 && look_L == false)
        {
            // マウスの位置がプレイヤーの上と左側の70度以内にある場合の処理
            spriteRenderer.sprite = upRightSprite;
            return;
        }
        if (Mathf.Abs(angle) <= 50)
        {
            // マウスの位置がプレイヤーの上90度以内にある場合の処理
            if (spriteRenderer && upSprite)
            {
                spriteRenderer.sprite = upSprite;
            }
            return;
        }
        if (Mathf.Abs(angle_down) <= 50)
        {
            // マウスの位置がプレイヤーの下90度以内にある場合の処理
            if (spriteRenderer && downSprite)
            {
                spriteRenderer.sprite = downSprite;
            }
            return;
        }
        if (angle_down > 40 && angle_down <= 130 && look_L == true)
        {
            // マウスの位置がプレイヤーの右側の90度以内にある場合の処理
            spriteRenderer.sprite = rightSprite;

            return;
        }
        if (angle > 40 && angle <= 130 && look_L == false)
        {
            // マウスの位置がプレイヤーの右側の90度以内にある場合で、かつプレイヤーが反転している時の処理
            spriteRenderer.sprite = rightSprite;

            return;
        }
        if (spriteRenderer && normalSprite)
        {
            // デフォルトの顔
            spriteRenderer.sprite = normalSprite;

            return;
        }
    }

    public void LookL(bool amount)
    {
        look_L = amount;
    }
}
