using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChecker : MonoBehaviour
{
    private bool isStay;
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
        if (isStay == false)
        {
            return;
        }
        if (eventBase != null && eventBase.isEventPlay == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            eventBase.isEventPlay = true;
            eventBase.ExecuteEvent();
        }
    }
}
