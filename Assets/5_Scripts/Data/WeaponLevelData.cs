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

    [LabelText("装備可能レベル"), HideLabel]
    public int canELevel = 1;

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

 
    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
