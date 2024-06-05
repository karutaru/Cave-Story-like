using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DataBase : MonoBehaviour
{
    public static DataBase instance;

    public ItemDataSO itemSO;
    public EnemyDataSO enemySO;
    public WeaponDataSO weaponSO;
    public EquipmentDataSO equipmentSO;
    public StageDataSO stageSO;

    [Header("エネミー 汎用")]
    public AudioClip deathSE;               // 死亡時のSE
    public GameObject expPrefabs;           // 経験値のプレハブ
    public GameObject damagePrefabs;        // ダメージテキストのプレハブ
    public GameObject ExplosionFirstPrefab; // 最初の大きな爆発のプレハブ
    public GameObject ExplosionPrefab;      // 次点の細かな爆発のプレハブ

    [Header("プレイヤー 汎用")]
    public Transform player_Target;                // プレイヤーターゲットの位置


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
    }

    public ItemData GetItemDatafromID(int searchID)
    {
        return itemSO.itemDataList.Find(itemData => itemData.itemID == searchID);
    }

}
