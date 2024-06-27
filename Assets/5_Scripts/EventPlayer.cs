using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypeEnum
{
    キャラクター, 扉, スイッチオブジェクト, 宝箱
}
public enum ChestTypeEnum
{
    直接獲得, ランダムドロップ
}
public enum ItemTypeEnum
{
    アイテム, 武器
}

public class EventPlayer : EventBase
{
    [Title("イベント設定"), EnumToggleButtons, HideLabel]
    public EventTypeEnum eventTypeEnumField;


    [ShowIf("eventTypeEnumField", EventTypeEnum.キャラクター)]
    [Title("キャラクター設定"), EnumToggleButtons, HideLabel]
    public GameObject chara_Event;

    [ShowIf("eventTypeEnumField", EventTypeEnum.扉)]
    [ValueDropdown("GetStageData")]
    public StageData selected_Stage;

    [ShowIf("eventTypeEnumField", EventTypeEnum.スイッチオブジェクト)]
    public GameObject selected_SwitchObject;

    [ShowIf("eventTypeEnumField", EventTypeEnum.宝箱)]
    [EnumToggleButtons, HideLabel]
    public ChestTypeEnum itemChestTypeEnumField;

    [ShowIf("itemChestTypeEnumField", ChestTypeEnum.直接獲得)]
    [LabelText("アイテムの種類"), HideLabel]
    [EnumToggleButtons]
    public ItemTypeEnum itemTypeEnumField;

    [ShowIf("itemTypeEnumField", ItemTypeEnum.アイテム)]
    [ValueDropdown("GetOneItemData")]
    public ItemData selected_OneItem;

    [ShowIf("itemTypeEnumField", ItemTypeEnum.武器)]
    [ValueDropdown("GetOneWeaponData")]
    public ItemData selected_OneWeapon;

    [ShowIf("itemChestTypeEnumField", ChestTypeEnum.ランダムドロップ)]
    [ValueDropdown("GetRandomItemData")]
    public ItemData selected_RandomItem;



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

    private List<ValueDropdownItem<ItemData>> GetOneItemData()
    {
        List<ValueDropdownItem<ItemData>> dropdownItems = new List<ValueDropdownItem<ItemData>>();

        if (itemDataReference != null && itemDataReference.itemDataList != null)
        {
            foreach (var itemData in itemDataReference.itemDataList)
            {
                dropdownItems.Add(new ValueDropdownItem<ItemData>(itemData.item_Name, itemData));
            }
        }

        return dropdownItems;
    }
    private List<ValueDropdownItem<WeaponData>> GetOneWeaponData()
    {
        List<ValueDropdownItem<WeaponData>> dropdownItems = new List<ValueDropdownItem<WeaponData>>();

        if (weaponDataReference != null && weaponDataReference.weaponDataList != null)
        {
            foreach (var weaponData in weaponDataReference.weaponDataList)
            {
                dropdownItems.Add(new ValueDropdownItem<WeaponData>(weaponData.gun_Name, weaponData));
            }
        }

        return dropdownItems;
    }
    private List<ValueDropdownItem<ItemData>> GetRandomItemData()
    {
        List<ValueDropdownItem<ItemData>> dropdownItems = new List<ValueDropdownItem<ItemData>>();

        if (itemDataReference != null && itemDataReference.itemDataList != null)
        {
            foreach (var itemData in itemDataReference.itemDataList)
            {
                dropdownItems.Add(new ValueDropdownItem<ItemData>(itemData.item_Name, itemData));
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

    [FoldoutGroup("データ参照", expanded: false)]
    public WeaponDataSO weaponDataReference;

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
        // chara_Eventをインスタンシエイト
        if (chara_Event != null)
        {
            Instantiate(chara_Event, transform.position, transform.rotation);
        }

        FinishEvent();
    }

    public override void SwitchObject()
    {

    }

    public override void Event_Item()
    {
        Debug.Log("Item");

        FinishEvent();
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
