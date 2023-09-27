using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum EquipmentEnum
{
    頭, 体, 足, アクセサリー
}

[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "Create EquipmentDataSO")]
public class EquipmentDataSO : ScriptableObject
{
    [Title("装備の部位"), EnumToggleButtons, HideLabel]
    public EquipmentEnum wideEnumField;

    [Title("頭"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.頭)]
    public List<EquipmentData> EquipmentDataList_Head = new List<EquipmentData>();

    [Title("体"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.体)]
    public List<EquipmentData> EquipmentDataList_Body = new List<EquipmentData>();

    [Title("足"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.足)]
    public List<EquipmentData> EquipmentDataList_Leg = new List<EquipmentData>();

    [Title("アクセサリー"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.アクセサリー)]
    public List<EquipmentData> EquipmentDataList_accessories = new List<EquipmentData>();
}
