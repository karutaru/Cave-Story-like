using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemInventryData
{
    public int itemID;
    public int amount;
}

public class ItemController : MonoBehaviour
{
    [SerializeField, Header("�A�C�e���f�[�^�̃X�N���v�^�u���I�u�W�F�N�g")]
    private ItemDataSO itemDataSO;

    public int playerGetItemID;

    // �A�C�e���̏��������Ǘ����鎫��
    public Dictionary<int, int> itemCounts = new Dictionary<int, int>();

    [SerializeField] private List<Text> itemCountTexts;
    [SerializeField] private List<ItemInventryData> itemInventryDataList = new List<ItemInventryData>();

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        DisplayInventory();
    //    }
    //}

    public void AddItem(int itemId, int amount)
    {
        if (!itemCounts.ContainsKey(itemId))
        {
            itemCounts[itemId] = 0;
        }

        itemCounts[itemId] += amount;

        UpdateItemCountText(itemId);
    }

    public int GetItemCount(int itemId)
    {
        if (itemCounts.ContainsKey(itemId))
        {
            Debug.Log(itemId);
            return itemCounts[itemId];
        }
        else
        {
            return 0;
        }
    }

    private void DisplayInventory()
    {
        Debug.Log("�A�C�e���ꗗ�F");
        foreach (var item in itemCounts)
        {
            Debug.Log("�A�C�e��ID: " + item.Key + " �A������: " + item.Value);
        }
    }

    private void UpdateItemCountText(int itemId)
    {
        if (itemDataSO != null)
        {
            ItemData itemData = itemDataSO.itemDataList.Find(item => item.item_ID == itemId);
            if (itemData != null)
            {
                int totalItemCount = TotalItemCount();
                float percentage = (float)itemCounts[itemId] / totalItemCount * 100;
                itemCountTexts[(int)itemData.item_Rarity].text = $"{itemData.item_Rarity}: ������: {itemCounts[itemId]} ({percentage:F1}%)";
            }
        }
    }

    private int TotalItemCount()
    {
        int total = 0;
        foreach (var item in itemCounts)
        {
            total += item.Value;
        }
        return total;
    }
}

