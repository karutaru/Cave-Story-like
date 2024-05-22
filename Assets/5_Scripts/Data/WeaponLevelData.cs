using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponLevelData
{
    public enum RARITY
    {
        �R����,         
        �A���R����,      
        ���A,           
        �G�s�b�N,       
        ���j�[�N       
    }



    [HorizontalGroup("Images", Width = 100)]
    [LabelText("����̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite gunImage;

    [HorizontalGroup("Images", Width = 100)]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [LabelText("     �A�C�R��"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [HorizontalGroup("Row 0")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [LabelText("�����e�̖��O����"), HideLabel, LabelWidth(100)]
    public string gunName;

    [HorizontalGroup("Row 1", Width = 0.5f)]
    [LabelText("�e��ID"), HideLabel, LabelWidth(100)]
    public string gunID;

    [HorizontalGroup("Row 1")]
    [LabelText("�����\���x��"), HideLabel, LabelWidth(100)]
    public int canELevel = 1;

    [HorizontalGroup("Row 2")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 8)]
    [LabelText("���A���e�B"), HideLabel, LabelWidth(100)]
    public RARITY rarity;



    [HorizontalGroup("Row 12")]
    [LabelText("�ő�_���[�W"), HideLabel, LabelWidth(100)]
    public int maxDamage;

    [HorizontalGroup("Row 13", Width = 0.5f)]
    [LabelText("�ŏ��_���[�W"), HideLabel, LabelWidth(100)]
    public int minDamage;

    [HorizontalGroup("Row 14", Width = 0.5f)]
    [LabelText("�ő�e��"), HideLabel, LabelWidth(100)]
    public int maxAmmo;

    [HorizontalGroup("Row 15")]
    [LabelText("�����[�h����"), HideLabel, LabelWidth(100)]
    public float reloadTime = 1;

    [HorizontalGroup("Row 16")]
    [LabelText("�N���e�B�J����"), HideLabel, LabelWidth(100)]
    public float criticalRate;


    [HorizontalGroup("Row 12")]
    [LabelText("�ˌ��Ԋu"), HideLabel, LabelWidth(100)]
    public float shotInterval = 80;

    [HorizontalGroup("Row 13", Width = 0.5f)]
    [LabelText("�e��"), HideLabel, LabelWidth(100)]
    public float shotSpeed = 10;

    [HorizontalGroup("Row 14", Width = 0.5f)]
    [LabelText("�e��������"), HideLabel, LabelWidth(100)]
    public float shotRange = 0.3f;

    [HorizontalGroup("Row 15", Width = 0.5f)]
    [LabelText("�ˌ����x"), HideLabel, LabelWidth(100)]
    public float shotAccuracy = 0.3f;

    [HorizontalGroup("Row 16")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
    [LabelText("���� �ˌ����x"), HideLabel, LabelWidth(100)]
    public float firstAccuracy = 0.3f;




    [HorizontalGroup("Row 27")]
    [LabelText("�e�̃v���n�u"), HideLabel, LabelWidth(100)]
    public BulletController gunShellPrefab;

    [HorizontalGroup("Row 27", Width = 0.5f)]
    [LabelText("�e��Y���I�t�Z�b�g"), HideLabel, LabelWidth(100)]
    public float prefabPositionOffsetY = 0;

    [HorizontalGroup("Row 28")]
    [LabelText("�e�̃T�C�Y"), HideLabel, LabelWidth(100)]
    public float shellSize = 1f;




    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
