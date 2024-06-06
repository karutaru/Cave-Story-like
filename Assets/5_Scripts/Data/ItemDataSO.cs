using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ScriptableObjects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, ShowIndexLabels = true, CustomAddFunction = "CreateNewItemData", ListElementLabelName = "item_Name")]
    public List<ItemData> itemDataList = new();

    private ItemData CreateNewItemData()
    {
        return new ItemData
        {
            item_ID = itemDataList.Count
        };
    }
}
