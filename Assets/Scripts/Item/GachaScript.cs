using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class GachaScript : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemDataSO;
    private int randomGachaValue;

    private Rigidbody2D rb;
    public bool isGachaStay = false;
    private ItemController itemController;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out ItemController itemController))
        {
            isGachaStay = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out ItemController itemController))
        {
            isGachaStay = false;
            itemController = null;
        }
    }

    void Update()
    {
        if (isGachaStay == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            randomGachaValue = Random.Range(1, 1001);
            ItemData selectedItem = RollRarity();
            if (selectedItem != null && selectedItem.itemPrefab != null)
            {
                ItemScript item = Instantiate(selectedItem.itemPrefab,
            new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0), transform.rotation);
            }
            else
            {
                Debug.LogWarning("選択されたアイテムまたはアイテムのプレハブがnull");
            }
        }
    }



    ItemData RollRarity()
    {
        List<ItemData> itemsByRarity;
        if (randomGachaValue <= 600) // コモン
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.コモン && index != 0).ToList();
        }
        else if (randomGachaValue <= 900) // アンコモン
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.アンコモン && index != 0).ToList();
        }
        else if (randomGachaValue <= 980) // レア
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.レア && index != 0).ToList();
        }
        else if (randomGachaValue <= 999) // エピック
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.エピック && index != 0).ToList();
        }
        else // ユニーク
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.ユニーク && index != 0).ToList();
        }

        if (itemsByRarity.Count > 0)
        {
            float totalRarityWeight = itemsByRarity.Sum(item => 1 / (float)item.rarityWait);
            float randomWeight = Random.Range(0f, totalRarityWeight);
            float currentWeight = 0f;
            for (int i = 0; i < itemsByRarity.Count; i++)
            {
                currentWeight += 1 / (float)itemsByRarity[i].rarityWait;
                if (randomWeight <= currentWeight)
                {
                    return itemsByRarity[i];
                }
            }
        }
        return null;
    }
}
