using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMessage : EventBase
{
    [SerializeField, Multiline(3)]
    protected string systemMessage;


    public override void ExecuteEvent()
    {
        //base.ExecuteEvent();
        //ゲーム画面にメッセージウィンドを表示
        Debug.Log(systemMessage);
        SystemTextManager.instanceSystemText.PrepareSystemMessage(systemMessage, GetComponent<EventBase>());
    }
}
