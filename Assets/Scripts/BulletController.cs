using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // �v���p�e�B

    public void Shoot(Vector2 direction, int damage)
    {
        // �_���[�W�Z�b�g
        weaponDamage = damage;

        // ����
        GetComponent<Rigidbody2D>().velocity = direction;
    }
}
