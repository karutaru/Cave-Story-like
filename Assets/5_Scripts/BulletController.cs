using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [LabelText("�ǂɓ�����Ə�����"), HideLabel]
    public bool isWallHit = true;

    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // �v���p�e�B

    private void Start()
    {
        if (!isWallHit)
        {
            // 2�ڂ�Circle Collider 2D��ON�ɂ���
            CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
            if (colliders.Length > 1)
            {
                colliders[1].enabled = true;
            }
        }
    }

    public void Shoot(Vector2 direction, int maxDamage, int minDamage) // --------------------------------------------------------------------------------------------------------------------------------------------
    {
        // �ő�l�ƍŏ��l����_���[�W���v�Z���A�_���[�W�Z�b�g
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // ����
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other) // �g���K�[�ɉ������G�ꂽ�� ----------------------------------------------------------------------------------------------------------------------------------------
    {
        if (other.CompareTag("Obstacle") && isWallHit) // ��Q����������
        {
            // �ǃG�t�F�N�g�����̉�����
            GameObject wallEffect = Instantiate(GameManager.game.wallEffectPrefab, transform.position, Quaternion.identity);
            // �ǃG�t�F�N�g��0.2�b��ɏ���
            Destroy(wallEffect, 0.5f);

            // �v���C���[�̒e��j��
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Breakable")) // �G�l�~�[�܂��͔j��\�I�u�W�F�N�g��������
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // �o���G�t�F�N�g�����X�g���̑S�Ẵv���n�u������̉����A�e�I�u�W�F�N�g��ݒ肷��
            foreach (var bloodEffectPrefab in GameManager.game.bloodEffectPrefabs)
            {
                GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
                bloodEffect.transform.SetParent(other.transform); // �e�I�u�W�F�N�g��ݒ�
                // �o���G�t�F�N�g��2�b��ɏ���
                Destroy(bloodEffect, 2f);
            }

            // �v���C���[�̒e��j��
            Destroy(this.gameObject, 0.01f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // �������Փ˂�����i��g���K�[�j----------------------------------------------------------------------------------------------------------------------------------------
    {
        if (collision.gameObject.CompareTag("Enemy") && !isWallHit || collision.gameObject.CompareTag("Breakable") && !isWallHit)
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // �o���G�t�F�N�g�����X�g���̑S�Ẵv���n�u������̉����A�e�I�u�W�F�N�g��ݒ肷��
            foreach (var bloodEffectPrefab in GameManager.game.bloodEffectPrefabs)
            {
                GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
                bloodEffect.transform.SetParent(collision.transform); // �e�I�u�W�F�N�g��ݒ�
                // �o���G�t�F�N�g��2�b��ɏ���
                Destroy(bloodEffect, 2f);
            }

            // �v���C���[�̒e��j��
            Destroy(this.gameObject);
        }
    }
}