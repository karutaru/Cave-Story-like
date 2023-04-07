using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // スクリプタブルオブジェクトの登録（アセットをアサイン）
    [SerializeField, Header("アイテムデータのスクリプタブルオブジェクト")]
    private ItemDataSO itemDataSO;

    public int playerGetItemID;


    public void AddItem(int amount)
    {

    }
}
