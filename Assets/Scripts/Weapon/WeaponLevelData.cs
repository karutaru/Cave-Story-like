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
    public float shotSpeed;
    public BulletController gunShellPrefab;
}
