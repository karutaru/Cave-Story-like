using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChecker : MonoBehaviour
{
    public bool isStay;
    private EventBase eventBase;


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out eventBase))
        {
            isStay = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out eventBase))
        {
            isStay = false;
            eventBase = null;
        }
    }

    void Update()
    {
        if (isStay == false || eventBase == null)
        {
            return;
        }
        if (eventBase.isEventPlay == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            eventBase.isEventPlay = true;
            eventBase.SwitchObject();           // 動作オブジェクト
        }
    }
}
