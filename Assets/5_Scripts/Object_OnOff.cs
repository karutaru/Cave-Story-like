using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Object_OnOff : EventBase
{
    [SpaceAttribute]
    [Header("-----------------------------------")]
    [SpaceAttribute]

    [SerializeField, Header("ここに対象のオブジェクトを追加")]
    private GameObject TargetSprite;
    [Header("変更前スプライト")]
    public Sprite originObj;
    [Header("変更後スプライト")]
    public Sprite changeObj;

    [SpaceAttribute]
    [Header("-----------------------------------")]
    [SpaceAttribute]

    [Header("ON時のSE")]
    public AudioClip isOnSE;
    [Header("OFF時のSE")]
    public AudioClip isOffSE;

    [SpaceAttribute]
    [Header("-----------------------------------")]
    [SpaceAttribute]

    [Header("スイッチ状況")]
    public bool switchObj = false;

    private SpriteRenderer spriteRenderer;
    AudioSource audioSource;


    private void Start()
    {
        if (!TryGetComponent(out spriteRenderer)) return;
        if (!TryGetComponent(out audioSource)) return;
    }


    public override void SwitchObject()
    {
        //動作オブジェクトを動かす

        // OFFの時
        if (switchObj)
        {
            if (spriteRenderer != null && originObj != null)
            {
                AudioSource.PlayClipAtPoint(isOffSE, transform.position);
                spriteRenderer.sprite = originObj;
                switchObj = false;
                isEventPlay = false;
            }
        } else
        {
            // Onの時
            if (spriteRenderer != null && changeObj != null)
            {
                AudioSource.PlayClipAtPoint(isOnSE, transform.position);
                spriteRenderer.sprite = changeObj;
                switchObj = true;
                isEventPlay = false;
            }
        }
    }
}

