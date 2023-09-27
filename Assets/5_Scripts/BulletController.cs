using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // �v���p�e�B

    public void Shoot(Vector2 direction, int maxDamage, int minDamage)
    {
        // �ő�l�ƍŏ��l����_���[�W���v�Z���A�_���[�W�Z�b�g
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // ����
        GetComponent<Rigidbody2D>().velocity = direction;
    }
}
