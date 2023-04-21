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
                Debug.LogWarning("�I�����ꂽ�A�C�e���܂��̓A�C�e���̃v���n�u��null");
            }
        }
    }



    ItemData RollRarity()
    {
        List<ItemData> itemsByRarity;
        if (randomGachaValue <= 600) // �R����
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.�R���� && index != 0).ToList();
        }
        else if (randomGachaValue <= 900) // �A���R����
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.�A���R���� && index != 0).ToList();
        }
        else if (randomGachaValue <= 980) // ���A
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.���A && index != 0).ToList();
        }
        else if (randomGachaValue <= 999) // �G�s�b�N
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.�G�s�b�N && index != 0).ToList();
        }
        else // ���j�[�N
        {
            itemsByRarity = itemDataSO.itemDataList.Where((item, index) => item.itemRarity == ItemRarity.���j�[�N && index != 0).ToList();
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
