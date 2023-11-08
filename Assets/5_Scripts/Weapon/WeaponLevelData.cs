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

    [LabelText("���x��"), HideLabel]
    public int level = 1;

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

    //// ----------------------------------------------�i��--------------------------------------------------------

    //[LabelText("�i������̉摜"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //[PreviewField(40)]
    //public Sprite gunImage_Evo;

    //[LabelText("���x��"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public int level_Evo = 1;

    //[LabelText("�o���l"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public int exp_Evo;

    //[LabelText("�ő�_���[�W"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public int maxDamage_Evo;

    //[LabelText("�ŏ��_���[�W"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public int minDamage_Evo;

    //[LabelText("�ő�e��"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public int maxAmmo_Evo;

    //[LabelText("�e��"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public float shotSpeed_Evo = 10;

    //[LabelText("�����[�h����"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public float reloadTime_Evo = 1;

    //[LabelText("�˒�"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public float shotRange_Evo = 0.3f;

    //[LabelText("�e�̃v���n�u"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public BulletController gunShellPrefab_Evo;

    //[LabelText("�v���n�u��Y���I�t�Z�b�g"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.�i��)]
    //public float prefabPositionOffsetY_Evo = 0;

    public override string ToString()
    {
        return $"{gunID} {gunName}";
    }
}
