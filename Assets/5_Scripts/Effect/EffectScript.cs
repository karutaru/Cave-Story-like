using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

public class EffectScript : MonoBehaviour
{
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

    void Start()
    {
        // �p�[�e�B�N���G�t�F�N�g��e�̈ʒu�ɃC���X�^���X�����A�e�̎q�I�u�W�F�N�g�ɂ���
        foreach (var prefab in particleEffectPrefabs)
        {
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            instance.transform.parent = transform;
            particleEffectInstances.Add(instance);

            Debug.Log("Particle effect instantiated at position: " + transform.position);

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

    public void OnDestroy()
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