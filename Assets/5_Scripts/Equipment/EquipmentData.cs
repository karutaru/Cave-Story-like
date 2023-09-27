using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EquipmentData
{
    [LabelText("�������̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite EquipmentImage;

    [LabelText("�@�����������̖��O������"), HideLabel]
    public string EquipmentName;

    [LabelText("�@�����̔ԍ�"), HideLabel]
    public int EquipmentNumber;

    [LabelText("�@HP---------------------------"), HideLabel]
    public int hp;

    [LabelText("�@�U����"), HideLabel]
    public int attackPower;

    [LabelText("�@�h���------------------------"), HideLabel]
    public int DefensePower;

    [LabelText("�@�f����"), HideLabel]
    public int speed;

    [LabelText("�@�U���p�x----------------------"), HideLabel]
    public int attackSpeed;
}
