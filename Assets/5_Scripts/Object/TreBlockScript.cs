using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreBlockScript : MonoBehaviour
{
    public GameObject rockShard;


    // �v���C���[�̒e���Ԃ�������
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //if (bulletController == col.GetComponent<BulletController>())
        if (col.TryGetComponent(out BulletController bulletController))
        {
            // �v���C���[�̒e�����������ʒu�Ƃ̍������v�Z
            Vector2 hitDiff = transform.position - bulletController.transform.position;

            // ��̃J�P�����΂�
            if (rockShard.TryGetComponent(out Rigidbody2D rb))
            {
                int randomValue = Random.Range(30, 40);
                int randomUpValue = Random.Range(3, 8);
                rb.AddForce(Vector2.up * 0.2f * randomValue, ForceMode2D.Impulse);
                rb.AddForce(hitDiff.normalized * 0.2f * randomUpValue, ForceMode2D.Impulse);
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
