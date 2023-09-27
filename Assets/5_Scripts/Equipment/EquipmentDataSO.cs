using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum EquipmentEnum
{
    ��, ��, ��, �A�N�Z�T���[
}

[CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "Create EquipmentDataSO")]
public class EquipmentDataSO : ScriptableObject
{
    [Title("�����̕���"), EnumToggleButtons, HideLabel]
    public EquipmentEnum wideEnumField;

    [Title("��"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.��)]
    public List<EquipmentData> EquipmentDataList_Head = new List<EquipmentData>();

    [Title("��"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.��)]
    public List<EquipmentData> EquipmentDataList_Body = new List<EquipmentData>();

    [Title("��"), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.��)]
    public List<EquipmentData> EquipmentDataList_Leg = new List<EquipmentData>();

    [Title("�A�N�Z�T���["), HideLabel]
    [ShowIf("wideEnumField", EquipmentEnum.�A�N�Z�T���[)]
    public List<EquipmentData> EquipmentDataList_accessories = new List<EquipmentData>();
}
