using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity
{
    コモン,         // 約60%
    アンコモン,      // 約30%
    レア,           // 約8%
    エピック,       // 約1.9%
    ユニーク        // 約0.1%
}
[System.Serializable]
public class ItemData
{
    public int itemID;
    public string name;
    public ItemRarity itemRarity;
    public int rarityWait = 1; // 同レアリティ何個分の価値か
    [SerializeField, Multiline(3)]
    public string explanation;
    public int BuyingPrice;
    public int sellingPrice;
    public ItemScript itemPrefab;
    public Sprite itemSprite;
}
