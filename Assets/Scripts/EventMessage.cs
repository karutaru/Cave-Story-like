using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMessage : EventBase
{
    [SerializeField]
    protected string message;

  public override void ExecuteEvent()
  {
    //base.ExecuteEvent();
    //ゲーム画面にメッセージウィンドを表示
    Debug.Log(message);
    DisplayManager.instance.PrepareDisplayMessage(message, GetComponent<EventBase>());
  }
}
