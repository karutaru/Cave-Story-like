using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum WeaponEvolutionEnum
{
    �ʏ�, �i��
}

[System.Serializable]
public class WeaponLevelData
{
    [LabelText("�����������e�̖��O��������"), HideLabel]
    public string gunName;

    [Title("����̏��"), EnumToggleButtons, HideLabel]
    public WeaponEvolutionEnum weaponEvolutionEnumField;

    [LabelText("����̉摜"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    [PreviewField(40)]
    public Sprite gunImage;

    [LabelText("���x��"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public int level = 1;

    [LabelText("�o���l"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public int exp;

    [LabelText("�ő�_���[�W"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public int maxDamage;

    [LabelText("�ŏ��_���[�W"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public int minDamage;

    [LabelText("�ő�e��"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public int maxAmmo;

    [LabelText("�e��"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public float shotSpeed = 10;

    [LabelText("�����[�h����"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public float reloadTime = 1;

    [LabelText("�˒�"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public float shotRange = 0.3f;

    [LabelText("�e�̃v���n�u"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public BulletController gunShellPrefab;

    [LabelText("�v���n�u��Y���I�t�Z�b�g"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�ʏ�)]
    public float prefabPositionOffsetY = 0;

    // ----------------------------------------------�i��--------------------------------------------------------

    [LabelText("�i������̉摜"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    [PreviewField(40)]
    public Sprite gunImage_Evo;

    [LabelText("���x��"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public int level_Evo = 1;

    [LabelText("�o���l"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public int exp_Evo;

    [LabelText("�ő�_���[�W"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public int maxDamage_Evo;

    [LabelText("�ŏ��_���[�W"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public int minDamage_Evo;

    [LabelText("�ő�e��"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public int maxAmmo_Evo;

    [LabelText("�e��"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public float shotSpeed_Evo = 10;

    [LabelText("�����[�h����"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public float reloadTime_Evo = 1;

    [LabelText("�˒�"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public float shotRange_Evo = 0.3f;

    [LabelText("�e�̃v���n�u"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public BulletController gunShellPrefab_Evo;

    [LabelText("�v���n�u��Y���I�t�Z�b�g"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    public float prefabPositionOffsetY_Evo = 0;
}
