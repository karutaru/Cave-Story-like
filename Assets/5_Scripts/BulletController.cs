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

    [LabelText("�p�[�e�B�N���G�t�F�N�g�̃v���n�u"), HideLabel]
    public GameObject[] particleEffectPrefabs; // �p�[�e�B�N���G�t�F�N�g�̃v���n�u�̔z��
    [LabelText("�e�j���̃p�[�e�B�N���G�t�F�N�g�̃v���n�u"), HideLabel]
    public GameObject[] particleEffectPrefabs_Last; // �e�j���̃p�[�e�B�N���G�t�F�N�g�̃v���n�u�̔z��

    [LabelText("�p�[�e�B�N���G�t�F�N�g�̔����ꏊ�����[���h��Ԃɂ���"), HideLabel]
    public bool isWorldSpaceEffect = true;  // �p�[�e�B�N���G�t�F�N�g�̔����ꏊ�����[���h��Ԃɂ���

    [LabelText("�傫���̐ݒ���@"), HideLabel]
    public SizeMode sizeMode = SizeMode.Constant; // �傫���̐ݒ���@

    [ShowIf("sizeMode", SizeMode.Constant)]
    [LabelText("�{��"), HideLabel]
    public float sizeMultiplier = 1.0f; // �傫���̔{��

    [ShowIf("sizeMode", SizeMode.RandomBetweenTwoConstants)]
    [LabelText("�ŏ��{��"), HideLabel]
    public float minSize = 0.5f; // �ŏ��{��
    [ShowIf("sizeMode", SizeMode.RandomBetweenTwoConstants)]
    [LabelText("�ő�{��"), HideLabel]
    public float maxSize = 1.5f; // �ő�{��

    [ShowIf("sizeMode", SizeMode.Curve)]
    [LabelText("�T�C�Y�J�[�u"), HideLabel]
    public AnimationCurve sizeCurve = AnimationCurve.Linear(0, 1, 1, 1); // �T�C�Y�J�[�u

    // �p�[�e�B�N���G�t�F�N�g�̃C���X�^���X�̃��X�g
    private List<GameObject> particleEffectInstances = new List<GameObject>();
    // �e�j���̃p�[�e�B�N���G�t�F�N�g�̃C���X�^���X�̃��X�g
    private List<GameObject> particleEffectInstances_Last = new List<GameObject>();

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

        // �p�[�e�B�N���G�t�F�N�g��e�̈ʒu�ɃC���X�^���X�����A�e�̎q�I�u�W�F�N�g�ɂ���
        foreach (var prefab in particleEffectPrefabs)
        {
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            instance.transform.parent = transform;
            particleEffectInstances.Add(instance);

            if (isWorldSpaceEffect)
            {
                // �p�[�e�B�N���V�X�e����Simulation Space��World�ɐݒ�
                var particleSystem = instance.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
                }
            }
        }
    }

    public void Shoot(Vector2 direction, int maxDamage, int minDamage)
    {
        // �ő�l�ƍŏ��l����_���[�W���v�Z���A�_���[�W�Z�b�g
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // ����
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") && isWallHit)
        {
            // �ǃG�t�F�N�g�����̉�����
            GameObject wallEffect = Instantiate(GameManager.game.wallEffectPrefab, transform.position, Quaternion.identity);
            // �ǃG�t�F�N�g��0.2�b��ɏ���
            Destroy(wallEffect, 0.5f);

            // EffectScript�̃C���X�^���X���擾���AisDestroy���\�b�h���Ăяo��
            isDestroy();

            // �v���C���[�̒e��j��
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Breakable"))
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

            // EffectScript�̃C���X�^���X���擾���AisDestroy���\�b�h���Ăяo��
            isDestroy();

            // �v���C���[�̒e��j��
            Destroy(this.gameObject, 0.01f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
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

            // EffectScript�̃C���X�^���X���擾���AisDestroy���\�b�h���Ăяo��
            isDestroy();

            // �v���C���[�̒e��j��
            Destroy(this.gameObject);
        }
    }

    public void isDestroy()
    {
        float size = sizeMultiplier;
        if (sizeMode == SizeMode.RandomBetweenTwoConstants)
        {
            size = Random.Range(minSize, maxSize);
        }
        else if (sizeMode == SizeMode.Curve)
        {
            size = sizeCurve.Evaluate(Time.time);
        }

        foreach (var prefab in particleEffectPrefabs_Last)
        {
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            instance.transform.localScale = new Vector3(size, size, size);
            particleEffectInstances_Last.Add(instance);
        }

        // �e���j�󂳂ꂽ�Ƃ��Ƀp�[�e�B�N���G�t�F�N�g�̐e�q�֌W���������A��莞�Ԍ�ɔj�󂷂�
        foreach (var instance in particleEffectInstances)
        {
            if (instance != null)
            {
                var particleSystem = instance.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    // �p�[�e�B�N���V�X�e�����~���A�V�����p�[�e�B�N���̐������~�߂�
                    particleSystem.Stop();

                    // �p�[�e�B�N���V�X�e���̐e�q�֌W������
                    instance.transform.parent = null;

                    // �p�[�e�B�N���̎�����҂��Ă���j��
                    Destroy(instance, particleSystem.main.startLifetime.constantMax);
                }
            }
        }
    }

    public enum SizeMode
    {
        Constant,
        RandomBetweenTwoConstants,
        Curve
    }
}