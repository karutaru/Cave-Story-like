using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening; // DOTween���g�p���邽�߂ɒǉ�

public class TreBlockScript : MonoBehaviour
{
    [Title("�v���n�u�ݒ�")]
    [Tooltip("�C���X�^���V�G�C�g����v���n�u�̃��X�g")]
    public GameObject[] rockShard;

    public GameObject miniRock;

    [Title("�u���b�N�̑ϋv�l")]
    [Tooltip("�u���b�N�̑ϋv�l")]
    public int treBlock = 100;

    // �����ʒu���L�^���邽�߂̕ϐ�
    private Vector3 initialPosition;

    void Start()
    {
        // �����ʒu���L�^
        initialPosition = transform.position;
    }

    // �v���C���[�̒e���Ԃ�������
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Shell") && col.TryGetComponent(out BulletController bulletController))
        {
            transform.position = initialPosition;

            // treBlock�ϐ��̐��l��bulletController.WeaponDamage�̐��l�����炷
            treBlock -= bulletController.WeaponDamage;

            // miniRock���C���X�^���V�G�C�g����1�b��ɔj��
            GameObject miniRockInstance = Instantiate(miniRock, transform.position, Quaternion.identity);
            Destroy(miniRockInstance, 1f);

            // ���݂�Tween���~���Ă���k����Tween���J�n
            transform.DOKill(); // ���݂�Tween���~
            transform.DOShakePosition(0.5f, strength: new Vector3(0.1f, 0.1f, 0), vibrato: 20, randomness: 90, snapping: false, fadeOut: true)
                     .OnComplete(() => transform.position = initialPosition); // �k�����I������珉���ʒu�ɖ߂�

            // treBlock�ϐ��̐��l��0�ȉ��ɂȂ�����
            if (treBlock <= 0)
            {
                // ��̃J�P�����΂�
                foreach (var shard in rockShard)
                {
                    GameObject shardInstance = Instantiate(shard, transform.position, Quaternion.identity);
                    if (shardInstance.TryGetComponent(out Rigidbody2D rb))
                    {
                        int randomValue = Random.Range(30, 40);
                        int randomUpValue = Random.Range(3, 8);
                        rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                        rb.AddForce((transform.position - bulletController.transform.position).normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
                    }
                }

                // ���S����SE�Đ�
                AudioSource.PlayClipAtPoint(DataBase.instance.deathSE, transform.position);

                // �ŏ��̑傫�Ȕ����G�t�F�N�g�𐶐�
                GameObject ExplosionFirstEffect = Instantiate(DataBase.instance.ExplosionFirstPrefab, transform.position, Quaternion.identity);
                // ���܂��Ȕ����G�t�F�N�g�𐶐�
                GameObject ExplosionEffect = Instantiate(DataBase.instance.ExplosionPrefab, transform.position, Quaternion.identity);

                // ���������G�t�F�N�g��j��
                Destroy(ExplosionFirstEffect, 0.4f);
                Destroy(ExplosionEffect, 1f);

                // ���̃I�u�W�F�N�g��j��
                Destroy(this.gameObject);
            }
        }
    }
}