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


    [FoldoutGroup("性能")]
    [LabelText("武器アイコンの画像"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [FoldoutGroup("性能")]
    [LabelText("装備可能レベル"), HideLabel]
    public int canELevel = 1;

    [FoldoutGroup("性能")]
    [LabelText("経験値"), HideLabel]
    public int exp;

    [FoldoutGroup("性能")]
    [LabelText("最大ダメージ"), HideLabel]
    public int maxDamage;

    [FoldoutGroup("性能")]
    [LabelText("最小ダメージ"), HideLabel]
    public int minDamage;

    [FoldoutGroup("性能")]
    [LabelText("クリティカル率"), HideLabel]
    public float criticalRate;

    [FoldoutGroup("性能")]
    [LabelText("最大弾数"), HideLabel]
    public int maxAmmo;

    [FoldoutGroup("性能")]
    [LabelText("射撃間隔"), HideLabel]
    public float shotInterval = 80;

    [FoldoutGroup("性能")]
    [LabelText("弾速"), HideLabel]
    public float shotSpeed = 10;

    [FoldoutGroup("性能")]
    [LabelText("リロード時間"), HideLabel]
    public float reloadTime = 1;

    [FoldoutGroup("性能")]
    [LabelText("射程"), HideLabel]
    public float shotRange = 0.3f;

    [FoldoutGroup("性能")]
    [LabelText("初期 射撃精度"), HideLabel]
    public float firstAccuracy = 0.3f;

    [FoldoutGroup("性能")]
    [LabelText("射撃精度"), HideLabel]
    public float shotAccuracy = 0.3f;

    [FoldoutGroup("性能")]
    [LabelText("弾のプレハブ"), HideLabel]
    public BulletController gunShellPrefab;

    [FoldoutGroup("性能")]
    [LabelText("プレハブのY軸オフセット"), HideLabel]
    public float prefabPositionOffsetY = 0;

    [FoldoutGroup("性能")]
    [LabelText("弾プレハブのサイズ"), HideLabel]
    public float shellSize = 1f;

 
    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
