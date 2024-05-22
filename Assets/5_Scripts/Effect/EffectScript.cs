using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public GameObject effect; // �G�t�F�N�g�̃Q�[���I�u�W�F�N�g

    public float effectInterva = 0.3f; // �G�t�F�N�g�̔����Ԋu
    public float effectScale = 1.0f; // �G�t�F�N�g�̑傫��
    public int effectFrequency = 0; // �G�t�F�N�g�̕\����(0�Ȃ疳��)
    public float effectRandomPos = 0.5f; // �G�t�F�N�g�̔����ʒu�̃����_����
    public float effectRandomScale = 0.2f; // �G�t�F�N�g�̑傫���̃����_����

    private int currentEffectCount = 0; // ���݂̃G�t�F�N�g�\����

    void Start()
    {
        StartCoroutine(SpawnEffects());
    }

    IEnumerator SpawnEffects()
    {
        while (effectFrequency == 0 || currentEffectCount < effectFrequency)
        {
            // �G�t�F�N�g�̃C���X�^���V�G�C�g
            GameObject newEffect = Instantiate(effect, GetRandomPosition(), Quaternion.identity);

            // �G�t�F�N�g�̑傫����ݒ�
            float randomScale = effectScale + Random.Range(-effectRandomScale, effectRandomScale);
            newEffect.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // �G�t�F�N�g�̕\���񐔂��J�E���g
            currentEffectCount++;

            // ���̃G�t�F�N�g�܂őҋ@
            yield return new WaitForSeconds(effectInterva);
        }
    }

    Vector3 GetRandomPosition()
    {
        // ���݂̈ʒu����Ƀ����_���Ȉʒu�𐶐�
        Vector3 randomPos = new Vector3(
            transform.position.x + Random.Range(-effectRandomPos, effectRandomPos),
            transform.position.y + Random.Range(-effectRandomPos, effectRandomPos),
            transform.position.z
        );
        return randomPos;
    }
}
