using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Create ScriptableObjects/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    // Odin�̑������g�p���ă��X�g��ҏW���₷������
    [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, ShowIndexLabels = true, ListElementLabelName = "gunName")]
    public List<WeaponData> weaponDataList = new();
}