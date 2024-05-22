using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeEnum
{
    �A�C�e��, ����, �厖�ȕ�
}


public class ModifiedChestScript : MonoBehaviour
{
    public ItemDataSO itemDataReference;

    public WeaponLevelDataSO weaponDataReference;



    [Title("�A�C�e���̎��"), EnumToggleButtons, HideLabel]
    public ItemTypeEnum itemTypeEnumField;

    [Title("�󔠂̃A�C�e��")]
    [ShowIf("itemTypeEnumField", ItemTypeEnum.�A�C�e��)]
    [ValueDropdown("GetAllItems")]
    public ItemData selectedItem;

    [Title("�󔠂̃A�C�e��")]
    [ShowIf("itemTypeEnumField", ItemTypeEnum.����)]
    [ValueDropdown("GetAllWeapons")]
    public WeaponLevelData selectedWeapon;


    private List<ItemData> GetAllItems()
    {
        // ������ItemDataSO�̃C���X�^���X���Q�Ƃ��āAitemDataList��Ԃ��B
        return itemDataReference.itemDataList;
    }

    private List<WeaponLevelData> GetAllWeapons()
    {
        // ������WeaponDataSO�̃C���X�^���X���Q�Ƃ��āAweaponDataList��Ԃ��B
        return weaponDataReference.weaponLevelDataList;
    }
}