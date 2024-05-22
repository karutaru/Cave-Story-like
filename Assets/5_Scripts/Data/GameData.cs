using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public List<ItemData> inventoryList = new();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ItemData item1 = DataBase.instance.GetItemDatafromID(5);
        inventoryList.Add(item1);
        inventoryList.Add(DataBase.instance.GetItemDatafromID(7));
    }
}
