using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EquipmentData
{
    [LabelText("★装備の画像"), HideLabel]
    [PreviewField(40)]
    public Sprite EquipmentImage;

    [LabelText("　■■■装備の名前■■■"), HideLabel]
    public string EquipmentName;

    [LabelText("　装備の番号"), HideLabel]
    public int EquipmentNumber;

    [LabelText("　HP---------------------------"), HideLabel]
    public int hp;

    [LabelText("　攻撃力"), HideLabel]
    public int attackPower;

    [LabelText("　防御力------------------------"), HideLabel]
    public int DefensePower;

    [LabelText("　素早さ"), HideLabel]
    public int speed;

    [LabelText("　攻撃頻度----------------------"), HideLabel]
    public int attackSpeed;
}
