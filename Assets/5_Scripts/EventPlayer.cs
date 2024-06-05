using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypeEnum
{
    エンティティ, 扉, スイッチオブジェクト, 宝箱
}

public class EventPlayer : EventBase
{
    [Title("イベント設定"), EnumToggleButtons, HideLabel]
    public EventTypeEnum eventTypeEnumField;

    [Title("扉")]
    [ShowIf("eventTypeEnumField", EventTypeEnum.扉)]
    [ValueDropdown("GetStageData")]
    public StageData selectedStage;

    

    private List<ValueDropdownItem<StageData>> GetStageData()
    {
        List<ValueDropdownItem<StageData>> dropdownItems = new List<ValueDropdownItem<StageData>>();

        if (stageDataReference != null && stageDataReference.stageDataList != null)
        {
            foreach (var stageData in stageDataReference.stageDataList)
            {
                dropdownItems.Add(new ValueDropdownItem<StageData>(stageData.stage_Name, stageData));
            }
        }

        return dropdownItems;
    }


    protected EventBase eventBase;

    [FoldoutGroup("データ参照", expanded: false)]
    public StageDataSO stageDataReference;

    [FoldoutGroup("データ参照", expanded: false)]
    public ItemDataSO itemDataReference;

    //-------------------------------------------------ここまで------------------------------------------------------------------


    /// <summary>
    /// イベントを実行
    /// </summary>
    public override void Event_Door()
    {

    }

    public override void Event_Entity()
    {

    }

    public override void SwitchObject()
    {

    }

    public override void Event_Item()
    {

    }

    public override void FinishEvent()
    {
        eventBase = GetComponent<EventBase>();
        eventBase.isEventPlay = false;
    }
}
