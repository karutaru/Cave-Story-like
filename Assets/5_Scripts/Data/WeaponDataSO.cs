using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Create ScriptableObjects/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    // Odinの属性を使用してリストを編集しやすくする
    [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, ShowIndexLabels = true, CustomAddFunction = "CreateNewWeaponData", ListElementLabelName = "gun_Name")]
    public List<WeaponData> weaponDataList = new();

    private WeaponData CreateNewWeaponData()
    {
        return new WeaponData
        {
            gun_ID = weaponDataList.Count
        };
    }
}