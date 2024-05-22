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


    [FoldoutGroup("«\")]
    [LabelText("íACRÌæ"), HideLabel]
    [PreviewField(40)]
    public Sprite gunIconImage;

    [FoldoutGroup("«\")]
    [LabelText("õÂ\x"), HideLabel]
    public int canELevel = 1;

    [FoldoutGroup("«\")]
    [LabelText("o±l"), HideLabel]
    public int exp;

    [FoldoutGroup("«\")]
    [LabelText("Åå_[W"), HideLabel]
    public int maxDamage;

    [FoldoutGroup("«\")]
    [LabelText("Å¬_[W"), HideLabel]
    public int minDamage;

    [FoldoutGroup("«\")]
    [LabelText("NeBJ¦"), HideLabel]
    public float criticalRate;

    [FoldoutGroup("«\")]
    [LabelText("Ååe"), HideLabel]
    public int maxAmmo;

    [FoldoutGroup("«\")]
    [LabelText("ËÔu"), HideLabel]
    public float shotInterval = 80;

    [FoldoutGroup("«\")]
    [LabelText("e¬"), HideLabel]
    public float shotSpeed = 10;

    [FoldoutGroup("«\")]
    [LabelText("[hÔ"), HideLabel]
    public float reloadTime = 1;

    [FoldoutGroup("«\")]
    [LabelText("Ëö"), HideLabel]
    public float shotRange = 0.3f;

    [FoldoutGroup("«\")]
    [LabelText("ú Ë¸x"), HideLabel]
    public float firstAccuracy = 0.3f;

    [FoldoutGroup("«\")]
    [LabelText("Ë¸x"), HideLabel]
    public float shotAccuracy = 0.3f;

    [FoldoutGroup("«\")]
    [LabelText("eÌvnu"), HideLabel]
    public BulletController gunShellPrefab;

    [FoldoutGroup("«\")]
    [LabelText("vnuÌY²ItZbg"), HideLabel]
    public float prefabPositionOffsetY = 0;

    [FoldoutGroup("«\")]
    [LabelText("evnuÌTCY"), HideLabel]
    public float shellSize = 1f;

 
    //public override string ToString()
    //{
    //    return $"{gunID} {gunName}";
    //}
}
