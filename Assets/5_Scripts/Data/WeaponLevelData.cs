using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponLevelData
{
    [LabelText("�����������e�̖��O��������"), HideLabel]
    public string gunName;

    [LabelText("�e��ID"), HideLabel]
    public string gunID;

    [LabelText("����̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite gunImage;


    [FoldoutGroup("���\")]
    [LabelText("����A�C�R���̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [FoldoutGroup("���\")]
    [LabelText("�����\���x��"), HideLabel]
    public int canELevel = 1;

    [FoldoutGroup("���\")]
    [LabelText("�o���l"), HideLabel]
    public int exp;

    [FoldoutGroup("���\")]
    [LabelText("�ő�_���[�W"), HideLabel]
    public int maxDamage;

    [FoldoutGroup("���\")]
    [LabelText("�ŏ��_���[�W"), HideLabel]
    public int minDamage;

    [FoldoutGroup("���\")]
    [LabelText("�N���e�B�J����"), HideLabel]
    public float criticalRate;

    [FoldoutGroup("���\")]
    [LabelText("�ő�e��"), HideLabel]
    public int maxAmmo;

    [FoldoutGroup("���\")]
    [LabelText("�ˌ��Ԋu"), HideLabel]
    public float shotInterval = 80;

    [FoldoutGroup("���\")]
    [LabelText("�e��"), HideLabel]
    public float shotSpeed = 10;

    [FoldoutGroup("���\")]
    [LabelText("�����[�h����"), HideLabel]
    public float reloadTime = 1;

    [FoldoutGroup("���\")]
    [LabelText("�˒�"), HideLabel]
    public float shotRange = 0.3f;

    [FoldoutGroup("���\")]
    [LabelText("���� �ˌ����x"), HideLabel]
    public float firstAccuracy = 0.3f;

    [FoldoutGroup("���\")]
    [LabelText("�ˌ����x"), HideLabel]
    public float shotAccuracy = 0.3f;

    [FoldoutGroup("���\")]
    [LabelText("�e�̃v���n�u"), HideLabel]
    public BulletController gunShellPrefab;

    [FoldoutGroup("���\")]
    [LabelText("�v���n�u��Y���I�t�Z�b�g"), HideLabel]
    public float prefabPositionOffsetY = 0;

    [FoldoutGroup("���\")]
    [LabelText("�e�v���n�u�̃T�C�Y"), HideLabel]
    public float shellSize = 1f;

 
    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
