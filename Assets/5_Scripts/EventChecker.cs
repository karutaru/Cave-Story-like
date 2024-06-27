using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChecker : MonoBehaviour
{
    public bool isStay;
    private EventBase eventBase;
    private List<EventBase> eventBases = new List<EventBase>();

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out EventBase eb))
        {
            if (!eventBases.Contains(eb))
            {
                eventBases.Add(eb);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out EventBase eb))
        {
            if (eventBases.Contains(eb))
            {
                eventBases.Remove(eb);
            }

            if (eventBase == eb)
            {
                isStay = false;
                eventBase.SetMaterialToNull();
                eventBase = null;
            }
        }
    }

    void Update()
    {
        if (eventBases.Count == 0)
        {
            isStay = false;
            eventBase = null;
            return;
        }

        // 最も近いオブジェクトを探す
        float closestDistance = Mathf.Infinity;
        EventBase closestEventBase = null;
        foreach (var eb in eventBases)
        {
            float distance = Vector2.Distance(transform.position, eb.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEventBase = eb;
            }
        }

        if (closestEventBase != eventBase)
        {
            if (eventBase != null)
            {
                eventBase.SetMaterialToNull();
            }

            eventBase = closestEventBase;
            eventBase.ChangeMaterialToTalkObject();
        }

        isStay = true;

        if (eventBase == null || eventBase.isEventPlay)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            eventBase.isEventPlay = true;
            // EventPlayerを取得し、eventTypeEnumFieldに基づいて適切なメソッドを呼び出す
            EventPlayer eventPlayer = eventBase as EventPlayer;
            if (eventPlayer != null)
            {
                switch (eventPlayer.eventTypeEnumField)
                {
                    case EventTypeEnum.キャラクター:
                        eventPlayer.Event_Entity();
                        break;
                    case EventTypeEnum.扉:
                        eventPlayer.Event_Door();
                        break;
                    case EventTypeEnum.スイッチオブジェクト:
                        eventPlayer.SwitchObject();
                        break;
                    case EventTypeEnum.宝箱:
                        eventPlayer.Event_Item();
                        break;
                }
            }
        }
    }
}