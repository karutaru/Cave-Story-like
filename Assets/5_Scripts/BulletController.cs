using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool wallHit = true;
    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // �v���p�e�B

    public void Shoot(Vector2 direction, int maxDamage, int minDamage)
    {
        // �ő�l�ƍŏ��l����_���[�W���v�Z���A�_���[�W�Z�b�g
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // ����
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other) // �Ȃɂ����Ԃ�������
    {
        if (other.CompareTag("Obstacle")) // ��Q����������
        {
            // �ǃG�t�F�N�g�����̉�����
            GameObject wallEffect = Instantiate(GameManager.game.wallEffectPrefab, transform.position, Quaternion.identity);
            // �ǃG�t�F�N�g��0.2�b��ɏ���
            Destroy(wallEffect, 0.2f);

            // bullet�ɃA�^�b�`����Ă���EffectScript��OnDestroy���\�b�h���Ăяo��
            EffectScript effectScript = this.GetComponent<EffectScript>();
            if (effectScript != null)
            {
                effectScript.OnDestroy();
            }

            // �v���C���[�̒e��j��
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Enemy")) // �G�l�~�[��������
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // �o���G�t�F�N�g�����̉�����
            GameObject bloodEffect = Instantiate(GameManager.game.bloodEffectPrefab, transform.position, Quaternion.identity);
            // �o���G�t�F�N�g��0.2�b��ɏ���
            Destroy(bloodEffect, 0.2f);

            // �v���C���[�̒e��j��
            Destroy(this.gameObject);
        }
    }
}
