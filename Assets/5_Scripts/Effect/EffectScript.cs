using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public GameObject particleEffectPrefab; // �p�[�e�B�N���G�t�F�N�g�̃v���n�u
    public bool isWorldSpaceEffect = true;  // �p�[�e�B�N���G�t�F�N�g�̔����ꏊ�����[���h��Ԃɂ���

    // �p�[�e�B�N���G�t�F�N�g�̃C���X�^���X
    private GameObject particleEffectInstance;

    void Start()
    {
        // �p�[�e�B�N���G�t�F�N�g��e�̈ʒu�ɃC���X�^���X�����A�e�̎q�I�u�W�F�N�g�ɂ���
        particleEffectInstance = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        particleEffectInstance.transform.parent = transform;

        if (isWorldSpaceEffect)
        {
            // �p�[�e�B�N���V�X�e����Simulation Space��World�ɐݒ�
            var particleSystem = particleEffectInstance.GetComponent<ParticleSystem>();
            var mainModule = particleSystem.main;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    public void OnDestroy()
    {
        // �e���j�󂳂ꂽ�Ƃ��Ƀp�[�e�B�N���G�t�F�N�g�̐e�q�֌W���������A��莞�Ԍ�ɔj�󂷂�
        if (particleEffectInstance != null)
        {
            var particleSystem = particleEffectInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                // �p�[�e�B�N���V�X�e�����~���A�V�����p�[�e�B�N���̐������~�߂�
                particleSystem.Stop();

                // �p�[�e�B�N���V�X�e���̐e�q�֌W������
                particleEffectInstance.transform.parent = null;

                // �p�[�e�B�N���̎�����҂��Ă���j��
                Destroy(particleEffectInstance, particleSystem.main.startLifetime.constantMax);
            }
        }
    }
}