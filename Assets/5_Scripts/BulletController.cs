using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // プロパティ

    public void Shoot(Vector2 direction, int maxDamage, int minDamage)
    {
        // 最大値と最小値からダメージを計算し、ダメージセット
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // 発射
        GetComponent<Rigidbody2D>().velocity = direction;
    }
}
