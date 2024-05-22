using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponLevelData
{
    public enum RARITY
    {
        コモン,         
        アンコモン,      
        レア,           
        エピック,       
        ユニーク       
    }



    [HorizontalGroup("Images", Width = 100)]
    [LabelText("武器の画像"), HideLabel]
    [PreviewField(40)]
    public Sprite gunImage;

    [HorizontalGroup("Images", Width = 100)]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [LabelText("     アイコン"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [HorizontalGroup("Row 0")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [LabelText("■■銃の名前■■"), HideLabel, LabelWidth(100)]
    public string gunName;

    [HorizontalGroup("Row 1", Width = 0.5f)]
    [LabelText("銃のID"), HideLabel, LabelWidth(100)]
    public string gunID;

    [HorizontalGroup("Row 1")]
    [LabelText("装備可能レベル"), HideLabel, LabelWidth(100)]
    public int canELevel = 1;

    [HorizontalGroup("Row 2")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 8)]
    [LabelText("レアリティ"), HideLabel, LabelWidth(100)]
    public RARITY rarity;



    [HorizontalGroup("Row 12")]
    [LabelText("最大ダメージ"), HideLabel, LabelWidth(100)]
    public int maxDamage;

    [HorizontalGroup("Row 13", Width = 0.5f)]
    [LabelText("最小ダメージ"), HideLabel, LabelWidth(100)]
    public int minDamage;

    [HorizontalGroup("Row 14", Width = 0.5f)]
    [LabelText("最大弾数"), HideLabel, LabelWidth(100)]
    public int maxAmmo;

    [HorizontalGroup("Row 15")]
    [LabelText("リロード時間"), HideLabel, LabelWidth(100)]
    public float reloadTime = 1;

    [HorizontalGroup("Row 16")]
    [LabelText("クリティカル率"), HideLabel, LabelWidth(100)]
    public float criticalRate;


    [HorizontalGroup("Row 12")]
    [LabelText("射撃間隔"), HideLabel, LabelWidth(100)]
    public float shotInterval = 80;

    [HorizontalGroup("Row 13", Width = 0.5f)]
    [LabelText("弾速"), HideLabel, LabelWidth(100)]
    public float shotSpeed = 10;

    [HorizontalGroup("Row 14", Width = 0.5f)]
    [LabelText("弾生存時間"), HideLabel, LabelWidth(100)]
    public float shotRange = 0.3f;

    [HorizontalGroup("Row 15", Width = 0.5f)]
    [LabelText("射撃精度"), HideLabel, LabelWidth(100)]
    public float shotAccuracy = 0.3f;

    [HorizontalGroup("Row 16")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
    [LabelText("初期 射撃精度"), HideLabel, LabelWidth(100)]
    public float firstAccuracy = 0.3f;




    [HorizontalGroup("Row 27")]
    [LabelText("弾のプレハブ"), HideLabel, LabelWidth(100)]
    public BulletController gunShellPrefab;

    [HorizontalGroup("Row 27", Width = 0.5f)]
    [LabelText("弾のY軸オフセット"), HideLabel, LabelWidth(100)]
    public float prefabPositionOffsetY = 0;

    [HorizontalGroup("Row 28")]
    [LabelText("弾のサイズ"), HideLabel, LabelWidth(100)]
    public float shellSize = 1f;




    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
