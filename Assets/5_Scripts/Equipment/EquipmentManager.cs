using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    private Sprite newSprite;
    public SpriteRenderer targetSpriteRenderer;  // �Ώۂ�SpriteRenderer

    [SerializeField, Header("�����f�[�^�̃X�N���v�^�u���I�u�W�F�N�g")]
    private EquipmentDataSO EquipmentDataList_Body;

    private EquipmentData currentEquipmentData;
    public EquipmentData CurrentEquipmentData => currentEquipmentData;

    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            newSprite = EquipmentDataList_Body.EquipmentDataList_Body[0].EquipmentImage;

            targetSpriteRenderer.sprite = newSprite;  // �Ώۂ�SpriteRenderer�̃X�v���C�g��ύX
        }
    }
}
