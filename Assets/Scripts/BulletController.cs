using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // プロパティ

    public void Shoot(Vector2 direction, int damage)
    {
        // ダメージセット
        weaponDamage = damage;

        // 発射
        GetComponent<Rigidbody2D>().velocity = direction;
    }
}
