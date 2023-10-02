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

    void CheckMouseAngle()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        float angle = Vector2.SignedAngle(transform.up, direction);
        float angle_down = Vector2.SignedAngle(-transform.up, direction);

        if (Mathf.Abs(angle) <= 50)
        {
            // マウスの位置がプレイヤーの上90度以内にある場合の処理
            if (spriteRenderer && upSprite)
            {
                spriteRenderer.sprite = upSprite;
            }
        }
        else
        {

            if (Mathf.Abs(angle_down) <= 50)
            {
                // マウスの位置がプレイヤーの下90度以内にある場合の処理
                if (spriteRenderer && downSprite)
                {
                    spriteRenderer.sprite = downSprite;
                }
            }
            else
            {
                if (angle_down > 40 && angle_down <= 130 && look_L == true)
                {
                    // マウスの位置がプレイヤーの右側の90度以内にある場合の処理
                    spriteRenderer.sprite = rightSprite;
                }
                else
                {
                    if (angle > 40 && angle <= 130 && look_L == false)
                    {
                        // マウスの位置がプレイヤーの右側の90度以内にある場合の処理
                        spriteRenderer.sprite = rightSprite;
                    }
                    else
                    {
                        if (spriteRenderer && normalSprite)
                        {
                            spriteRenderer.sprite = normalSprite;
                        }
                    }
                }
            }
        }
    }

    public void LookL(bool amount)
    {
        look_L = amount;
    }
}
