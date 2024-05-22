using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponLevelData
{
    [LabelText("¡¡¡¡¡eÌ¼O¡¡¡¡"), HideLabel]
    public string gunName;

    [LabelText("eÌID"), HideLabel]
    public string gunID;

    [LabelText("íÌæ"), HideLabel]
    [PreviewField(40)]
    public Sprite gunImage;


    [LabelText("íACRÌæ"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [LabelText("õÂ\x"), HideLabel]
    public int canELevel = 1;

    [LabelText("o±l"), HideLabel]
    public int exp;

    [LabelText("Åå_[W"), HideLabel]
    public int maxDamage;

    [LabelText("Å¬_[W"), HideLabel]
    public int minDamage;

    [LabelText("NeBJ¦"), HideLabel]
    public float criticalRate;

    [LabelText("Ååe"), HideLabel]
    public int maxAmmo;

    [LabelText("ËÔu"), HideLabel]
    public float shotInterval = 80;

    [LabelText("e¬"), HideLabel]
    public float shotSpeed = 10;

    [LabelText("[hÔ"), HideLabel]
    public float reloadTime = 1;

    [LabelText("Ëö"), HideLabel]
    public float shotRange = 0.3f;

    [LabelText("ú Ë¸x"), HideLabel]
    public float firstAccuracy = 0.3f;

    [LabelText("Ë¸x"), HideLabel]
    public float shotAccuracy = 0.3f;

    [LabelText("eÌvnu"), HideLabel]
    public BulletController gunShellPrefab;

    [LabelText("vnuÌY²ItZbg"), HideLabel]
    public float prefabPositionOffsetY = 0;

    [LabelText("evnuÌTCY"), HideLabel]
    public float shellSize = 1f;

 
    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
