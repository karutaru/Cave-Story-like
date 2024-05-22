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


    [LabelText("����A�C�R���̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [LabelText("�����\���x��"), HideLabel]
    public int canELevel = 1;

    [LabelText("�o���l"), HideLabel]
    public int exp;

    [LabelText("�ő�_���[�W"), HideLabel]
    public int maxDamage;

    [LabelText("�ŏ��_���[�W"), HideLabel]
    public int minDamage;

    [LabelText("�N���e�B�J����"), HideLabel]
    public float criticalRate;

    [LabelText("�ő�e��"), HideLabel]
    public int maxAmmo;

    [LabelText("�ˌ��Ԋu"), HideLabel]
    public float shotInterval = 80;

    [LabelText("�e��"), HideLabel]
    public float shotSpeed = 10;

    [LabelText("�����[�h����"), HideLabel]
    public float reloadTime = 1;

    [LabelText("�˒�"), HideLabel]
    public float shotRange = 0.3f;

    [LabelText("���� �ˌ����x"), HideLabel]
    public float firstAccuracy = 0.3f;

    [LabelText("�ˌ����x"), HideLabel]
    public float shotAccuracy = 0.3f;

    [LabelText("�e�̃v���n�u"), HideLabel]
    public BulletController gunShellPrefab;

    [LabelText("�v���n�u��Y���I�t�Z�b�g"), HideLabel]
    public float prefabPositionOffsetY = 0;

    [LabelText("�e�v���n�u�̃T�C�Y"), HideLabel]
    public float shellSize = 1f;

 
    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
