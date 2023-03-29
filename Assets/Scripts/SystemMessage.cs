using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMessage : EventBase
{
    [SerializeField, Multiline(3)]
    protected string systemMessage;
    [Header("回復ポイント")]
    public bool heal = false;
    [Header("店")]
    public bool shop = false;


    public override void ExecuteEvent()
    {
        //base.ExecuteEvent();
        //ゲーム画面にメッセージウィンドを表示
        //Debug.Log(systemMessage);
        SystemTextManager.instanceSystemText.PrepareSystemMessage(systemMessage, GetComponent<EventBase>());
    }

    public override void ExecuteSelect()
    {
        //base.ExecuteEvent();
        //ゲーム画面にメッセージウィンドを表示
        //Debug.Log(systemMessage);
        SystemTextManager.instanceSystemText.PrepareSystemSelect(heal, shop, GetComponent<EventBase>());
    }
}
