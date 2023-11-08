using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HaveWeaponDataSO", menuName = "Create ScriptableObjects/HaveWeaponDataSO")]
public class HaveWeaponDataSO : ScriptableObject
{
    public List<HaveWeaponData> haveWeaponDataList = new();
}
