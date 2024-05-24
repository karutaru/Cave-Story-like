using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMessage : EventBase
{
    protected EventBase eventBase;

    //-------------------------------------------------ここまで------------------------------------------------------------------


    /// <summary>
    /// イベントを実行
    /// </summary>
    public override void ExecuteEvent()
    {

    }



    public override void FinishEvent()
    {
        eventBase = GetComponent<EventBase>();
        eventBase.isEventPlay = false;
    }
}
