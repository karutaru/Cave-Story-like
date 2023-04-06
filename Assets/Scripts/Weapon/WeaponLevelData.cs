using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponLevelData
{
    public int level;
    public int exp;
    public int damage;
    public int maxAmmo;
    public float shotSpeed = 10;
    public float reloadTime = 1;
    public float shotRange = 0.3f;
    public BulletController gunShellPrefab;
    public float prefabPositionOffsetY = 0;
}
