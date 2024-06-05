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

    [Header("�G�l�~�[ �ėp")]
    public AudioClip deathSE;               // ���S����SE
    public GameObject expPrefabs;           // �o���l�̃v���n�u
    public GameObject damagePrefabs;        // �_���[�W�e�L�X�g�̃v���n�u
    public GameObject ExplosionFirstPrefab; // �ŏ��̑傫�Ȕ����̃v���n�u
    public GameObject ExplosionPrefab;      // ���_�ׂ̍��Ȕ����̃v���n�u

    [Header("�v���C���[ �ėp")]
    public Transform player_Target;                // �v���C���[�^�[�Q�b�g�̈ʒu


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
