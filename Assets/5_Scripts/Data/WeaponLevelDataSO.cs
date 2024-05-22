using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponLevelDataSO", menuName = "Create ScriptableObjects/WeaponLevelDataSO")]
public class WeaponLevelDataSO : ScriptableObject
{
    public List<WeaponLevelData> weaponLevelDataList = new();
}