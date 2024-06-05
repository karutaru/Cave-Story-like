using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ChestScript : MonoBehaviour
{
    public ItemDataSO itemDataReference;

    public WeaponDataSO weaponDataReference;



    [Title("アイテムの種類"), EnumToggleButtons, HideLabel]
    public ItemTypeEnum itemTypeEnumField;

    [Title("宝箱のアイテム")]
    [ShowIf("itemTypeEnumField", ItemTypeEnum.アイテム)]
    [ValueDropdown("GetAllItems")]
    public ItemData selectedItem;

    [Title("宝箱のアイテム")]
    [ShowIf("itemTypeEnumField", ItemTypeEnum.武器)]
    [ValueDropdown("GetAllWeapons")]
    public WeaponData selectedWeapon;


    private List<ItemData> GetAllItems()
    {
        // ここでItemDataSOのインスタンスを参照して、itemDataListを返す。
        return itemDataReference.itemDataList;
    }

    private List<WeaponData> GetAllWeapons()
    {
        // ここでWeaponDataSOのインスタンスを参照して、weaponDataListを返す。
        return weaponDataReference.weaponDataList;
    }
}
