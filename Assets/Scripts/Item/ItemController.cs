using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    // スクリプタブルオブジェクトの登録（アセットをアサイン）
    [SerializeField, Header("アイテムデータのスクリプタブルオブジェクト")]
    private ItemDataSO itemDataSO;

    public int playerGetItemID;

    // アイテムの所持数を管理する辞書
    public Dictionary<int, int> itemCounts = new Dictionary<int, int>();

    [SerializeField] private List<Text> itemCountTexts;

    //-----------------------------------------ここまで--------------------------------------------------

    private void Update()
    {
        // E キーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.E))
        {
            DisplayInventory();


        }
    }

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
        Debug.Log("アイテム一覧：");
        foreach (var item in itemCounts)
        {
            Debug.Log("アイテムID: " + item.Key + " 、所持数: " + item.Value);
        }
    }

    private void UpdateItemCountText(int itemId)
    {
        if (itemDataSO != null)
        {
            ItemData itemData = itemDataSO.itemDataList.Find(item => item.itemID == itemId);
            if (itemData != null)
            {
                int index = itemDataSO.itemDataList.IndexOf(itemData);
                if (index >= 0 && index < itemCountTexts.Count && itemCountTexts[index] != null)
                {
                    int totalItemCount = TotalItemCount();
                    float percentage = (float)itemCounts[itemId] / totalItemCount * 100;
                    itemCountTexts[index].text = $"アイテムID: {itemId} 、所持数: {itemCounts[itemId]} ({percentage:F1}%)";
                }
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
