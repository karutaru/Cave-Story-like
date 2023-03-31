using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMessage : EventBase
{
    [SerializeField, Multiline(3)]
    protected string[] talks;
    [SerializeField, Multiline(3)]
    protected string message;

    [SerializeField]
    protected bool playerFace = false;
    [SerializeField]
    protected bool doctorFace = false;


    public override void ExecuteEvent()
    {
        //base.ExecuteEvent();
        //ゲーム画面にメッセージウィンドを表示
        //Debug.Log(message);
        DisplayManager.instance.PrepareDisplayMessage(message, playerFace, doctorFace, GetComponent<EventBase>());
    }
}
