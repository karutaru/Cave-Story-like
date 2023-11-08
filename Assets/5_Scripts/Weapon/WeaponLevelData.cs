using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponLevelData
{
    [LabelText("■■■■■銃の名前■■■■"), HideLabel]
    public string gunName;

    [LabelText("銃のID"), HideLabel]
    public string gunID;

    [LabelText("武器の画像"), HideLabel]
    [PreviewField(40)]
    public Sprite gunImage;

    [LabelText("武器アイコンの画像"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [LabelText("レベル"), HideLabel]
    public int level = 1;

    [LabelText("経験値"), HideLabel]
    public int exp;

    [LabelText("最大ダメージ"), HideLabel]
    public int maxDamage;

    [LabelText("最小ダメージ"), HideLabel]
    public int minDamage;

    [LabelText("クリティカル率"), HideLabel]
    public float criticalRate;

    [LabelText("最大弾数"), HideLabel]
    public int maxAmmo;

    [LabelText("射撃間隔"), HideLabel]
    public float shotInterval = 80;

    [LabelText("弾速"), HideLabel]
    public float shotSpeed = 10;

    [LabelText("リロード時間"), HideLabel]
    public float reloadTime = 1;

    [LabelText("射程"), HideLabel]
    public float shotRange = 0.3f;

    [LabelText("初期 射撃精度"), HideLabel]
    public float firstAccuracy = 0.3f;

    [LabelText("射撃精度"), HideLabel]
    public float shotAccuracy = 0.3f;

    [LabelText("弾のプレハブ"), HideLabel]
    public BulletController gunShellPrefab;

    [LabelText("プレハブのY軸オフセット"), HideLabel]
    public float prefabPositionOffsetY = 0;

    [LabelText("弾プレハブのサイズ"), HideLabel]
    public float shellSize = 1f;

    //// ----------------------------------------------進化--------------------------------------------------------

    //[LabelText("進化武器の画像"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //[PreviewField(40)]
    //public Sprite gunImage_Evo;

    //[LabelText("レベル"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public int level_Evo = 1;

    //[LabelText("経験値"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public int exp_Evo;

    //[LabelText("最大ダメージ"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public int maxDamage_Evo;

    //[LabelText("最小ダメージ"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public int minDamage_Evo;

    //[LabelText("最大弾数"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public int maxAmmo_Evo;

    //[LabelText("弾速"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public float shotSpeed_Evo = 10;

    //[LabelText("リロード時間"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public float reloadTime_Evo = 1;

    //[LabelText("射程"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public float shotRange_Evo = 0.3f;

    //[LabelText("弾のプレハブ"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public BulletController gunShellPrefab_Evo;

    //[LabelText("プレハブのY軸オフセット"), HideLabel]
    //[ShowIf("weaponEvolutionEnumField", WeaponEvolutionEnum.進化)]
    //public float prefabPositionOffsetY_Evo = 0;

    public override string ToString()
    {
        return $"{gunID} {gunName}";
    }
}
