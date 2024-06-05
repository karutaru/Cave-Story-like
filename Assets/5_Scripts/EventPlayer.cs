using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypeEnum
{
    キャラクター, 扉, スイッチオブジェクト, 宝箱
}

public class EventPlayer : EventBase
{
    [Title("イベント設定"), EnumToggleButtons, HideLabel]
    public EventTypeEnum eventTypeEnumField;


    [ShowIf("eventTypeEnumField", EventTypeEnum.キャラクター)]
    [Title("キャラクター設定"), EnumToggleButtons, HideLabel]

    [ShowIf("eventTypeEnumField", EventTypeEnum.扉)]
    [ValueDropdown("GetStageData")]
    public StageData selected_Stage;

    [ShowIf("eventTypeEnumField", EventTypeEnum.スイッチオブジェクト)]
    public GameObject selected_SwitchObject;

    [ShowIf("eventTypeEnumField", EventTypeEnum.宝箱)]
    [ValueDropdown("GetChest")]
    public ItemData selected_Item;



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

    private Material talkMaterial;
    private Material materialInstance; // 最初のマテリアルを保存する変数
    private Renderer eventRenderer;

    [FoldoutGroup("データ参照", expanded: false)]
    public StageDataSO stageDataReference;

    [FoldoutGroup("データ参照", expanded: false)]
    public ItemDataSO itemDataReference;

    //-------------------------------------------------ここまで------------------------------------------------------------------

    private void Start()
    {
        talkMaterial = GameManager.game.talkObject;
        eventRenderer = GetComponent<Renderer>();
        if (eventRenderer != null)
        {
            materialInstance = eventRenderer.material; // 最初のマテリアルを保存
        }
    }

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

    /// <summary>
    /// オブジェクトのマテリアルをGameManager.game.talkObjectに切り替える
    /// </summary>
    public override void ChangeMaterialToTalkObject()
    {
        if (eventRenderer != null)
        {
            eventRenderer.material = talkMaterial;
        }
    }

    /// <summary>
    /// オブジェクトのマテリアルをmaterialInstanceにする
    /// </summary>
    public override void SetMaterialToNull()
    {
        if (eventRenderer != null)
        {
            eventRenderer.material = materialInstance; // 最初のマテリアルに置き換える
        }
    }
}
