using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item_Rarity
{
    �R����,         // ��60%
    �A���R����,      // ��30%
    ���A,           // ��8%
    �G�s�b�N,       // ��1.9%
    ���j�[�N        // ��0.1%
}
[System.Serializable]
public class ItemData
{
    public int item_ID;
    public string item_Name;
    public Item_Rarity item_Rarity;
    public int rarityWait = 1; // �����A���e�B�����̉��l��
    [SerializeField, Multiline(3)]
    public string explanation; // ����
    public int BuyingPrice;
    public int sellingPrice;
    public ItemScript itemPrefab;
    public Sprite itemSprite;

    public override string ToString()
    {
        return $"{item_ID} {item_Name}";
    }
}
