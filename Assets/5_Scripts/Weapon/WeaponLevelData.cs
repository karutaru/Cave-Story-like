using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum WeaponEvolutionEnum
{
    通常, 進化
}

[System.Serializable]
public class WeaponLevelData
{
    [LabelText("■■■■■銃の名前■■■■"), HideLabel]
    public string gunName;

    [Title("武器の状態"), EnumToggleButtons, HideLabel]
    public WeaponEvolutionEnum weaponEvolutionEnumField;

    [LabelText("武器の画像"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    [PreviewField(40)]
    public Sprite gunImage;

    [LabelText("レベル"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public int level = 1;

    [LabelText("経験値"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public int exp;

    [LabelText("最大ダメージ"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public int maxDamage;

    [LabelText("最小ダメージ"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public int minDamage;

    [LabelText("最大弾数"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public int maxAmmo;

    [LabelText("弾速"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public float shotSpeed = 10;

    [LabelText("リロード時間"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public float reloadTime = 1;

    [LabelText("射程"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public float shotRange = 0.3f;

    [LabelText("弾のプレハブ"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public BulletController gunShellPrefab;

    [LabelText("プレハブのY軸オフセット"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.通常)]
    public float prefabPositionOffsetY = 0;

    // ----------------------------------------------進化--------------------------------------------------------

    [LabelText("進化武器の画像"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    [PreviewField(40)]
    public Sprite gunImage_Evo;

    [LabelText("レベル"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public int level_Evo = 1;

    [LabelText("経験値"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public int exp_Evo;

    [LabelText("最大ダメージ"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public int maxDamage_Evo;

    [LabelText("最小ダメージ"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public int minDamage_Evo;

    [LabelText("最大弾数"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public int maxAmmo_Evo;

    [LabelText("弾速"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public float shotSpeed_Evo = 10;

    [LabelText("リロード時間"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public float reloadTime_Evo = 1;

    [LabelText("射程"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public float shotRange_Evo = 0.3f;

    [LabelText("弾のプレハブ"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public BulletController gunShellPrefab_Evo;

    [LabelText("プレハブのY軸オフセット"), HideLabel]
    [ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    public float prefabPositionOffsetY_Evo = 0;
}
