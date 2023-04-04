using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Object_OnOff : EventBase
{
    [SpaceAttribute]
    [Header("-----------------------------------")]
    [SpaceAttribute]

    [SerializeField, Header("�����ɑΏۂ̃I�u�W�F�N�g��ǉ�")]
    private GameObject TargetSprite;
    [Header("�ύX�O�X�v���C�g")]
    public Sprite originObj;
    [Header("�ύX��X�v���C�g")]
    public Sprite changeObj;

    [SpaceAttribute]
    [Header("-----------------------------------")]
    [SpaceAttribute]

    [Header("ON����SE")]
    public AudioClip isOnSE;
    [Header("OFF����SE")]
    public AudioClip isOffSE;

    [SpaceAttribute]
    [Header("-----------------------------------")]
    [SpaceAttribute]

    [Header("�X�C�b�`��")]
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
        //����I�u�W�F�N�g�𓮂���

        // OFF�̎�
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
            // On�̎�
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

