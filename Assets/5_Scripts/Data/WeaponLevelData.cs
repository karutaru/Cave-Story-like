using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponLevelData
{
    [HorizontalGroup("Images", Width = 100)]
    [LabelText("����̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite gunImage;

    [HorizontalGroup("Images", Width = 100)]
    [LabelText("     �A�C�R��"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [LabelText("���������e�̖��O��������"), HideLabel]
    public string gunName;

    [HorizontalGroup("Row 1", Width = 0.5f)]
    [LabelText("�e��ID"), HideLabel, LabelWidth(100)]
    public string gunID;

    [HorizontalGroup("Row 1")]
    [LabelText("�����\���x��"), HideLabel, LabelWidth(100)]
    public int canELevel = 1;

    [HorizontalGroup("Row 2")]
    [LabelText("�e�̃v���n�u"), HideLabel, LabelWidth(100)]
    public BulletController gunShellPrefab;

    [HorizontalGroup("Row 2", Width = 0.5f)]
    [LabelText("�v���n�u��Y���I�t�Z�b�g"), HideLabel, LabelWidth(100)]
    public float prefabPositionOffsetY = 0;

    [HorizontalGroup("Row 3")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 8)]
    [LabelText("�e�v���n�u�̃T�C�Y"), HideLabel, LabelWidth(100)]
    public float shellSize = 1f;




    [HorizontalGroup("Row 4")]
    [LabelText("�ő�_���[�W"), HideLabel, LabelWidth(100)]
    public int maxDamage;

    [HorizontalGroup("Row 5", Width = 0.5f)]
    [LabelText("�ŏ��_���[�W"), HideLabel, LabelWidth(100)]
    public int minDamage;

    [HorizontalGroup("Row 6", Width = 0.5f)]
    [LabelText("�ő�e��"), HideLabel, LabelWidth(100)]
    public int maxAmmo;

    [HorizontalGroup("Row 7")]
    [LabelText("�ˌ��Ԋu"), HideLabel, LabelWidth(100)]
    public float shotInterval = 80;

    [HorizontalGroup("Row 8", Width = 0.5f)]
    [LabelText("�e��"), HideLabel, LabelWidth(100)]
    public float shotSpeed = 10;

    [HorizontalGroup("Row 4")]
    [LabelText("�����[�h����"), HideLabel, LabelWidth(100)]
    public float reloadTime = 1;

    [HorizontalGroup("Row 5", Width = 0.5f)]
    [LabelText("�e��������"), HideLabel, LabelWidth(100)]
    public float shotRange = 0.3f;

    [HorizontalGroup("Row 6", Width = 0.5f)]
    [LabelText("�ˌ����x"), HideLabel, LabelWidth(100)]
    public float shotAccuracy = 0.3f;

    [HorizontalGroup("Row 7")]
    [LabelText("���� �ˌ����x"), HideLabel, LabelWidth(100)]
    public float firstAccuracy = 0.3f;

    [HorizontalGroup("Row 8")]
    [LabelText("�N���e�B�J����"), HideLabel, LabelWidth(100)]
    public float criticalRate;


    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
