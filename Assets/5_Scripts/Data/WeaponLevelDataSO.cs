using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WeaponLevelDataSO", menuName = "Create ScriptableObjects/WeaponLevelDataSO")]
public class WeaponLevelDataSO : ScriptableObject
{
    // Odin�̑������g�p���ă��X�g��ҏW���₷������
    [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, ShowIndexLabels = true, ListElementLabelName = "gunName")]
    public List<WeaponLevelData> weaponLevelDataList = new();
}