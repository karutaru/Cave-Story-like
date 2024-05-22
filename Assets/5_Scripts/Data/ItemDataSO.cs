using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ScriptableObjects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public List<ItemData> itemDataList = new();
}
