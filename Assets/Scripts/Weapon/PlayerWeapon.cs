using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̕�����̊Ǘ��N���X
/// </summary>
public class PlayerWeapon : MonoBehaviour
{
    // �X�N���v�^�u���I�u�W�F�N�g�̓o�^�i�A�Z�b�g���A�T�C���j
    [SerializeField, Header("���탌�x���f�[�^�̃X�N���v�^�u���I�u�W�F�N�g")]
    private WeaponLevelDataSO weaponLevelDataSO;

    private int currentExp; // ���݂̌o���l
    private int currentWeaponLevel; // ���݂̕��탌�x��

    private WeaponLevelData currentWeaponLevelData; // ���݂̕��탌�x���f�[�^�i�e�̑��x�A�_���[�W�A�v���n�u�j
    public WeaponLevelData CurrentWeaponLevelData => currentWeaponLevelData; // �v���p�e�B

    private int maxLevel = 0;
    [SerializeField]
    private Text playerGunLevel;


    /// <summary>
    /// ������̏�����
    /// </summary>
    public void Init()
    {
        // ������
        currentExp = 0;
        currentWeaponLevel = -1;

        // ����̃f�[�^�x�[�X�̍ő僌�x�����}�b�N�X�ɐݒ�
        maxLevel = weaponLevelDataSO.weaponLevelDataList[weaponLevelDataSO.weaponLevelDataList.Count - 1].level;

        // ���탌�x���̍X�V
        UpdateWeaponLevel();
    }

    /// <summary>
    /// �o���l���Z�ƕ��탌�x���̍X�V
    /// </summary>
    /// <param name="amount"></param>
    public void AddExp(int amount)
    {
        // �o���l���Z
        currentExp += amount;

        // ���탌�x���̍X�V
        UpdateWeaponLevel();
    }

    /// <summary>
    /// �o���l���Z�ƕ��탌�x���̍X�V
    /// ���̏����ɂ���āA1�x��1�ȏ�̕��탌�x���͉����Ȃ�
    /// 2�ȏ㉺����ꍇ�ɂ́A1����������悤�ɂ��A����������̍Œ�o���l��K�p����
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveExp(int amount)
    {
        // ������̌o���l�̎��Z�B���Z���ʂ� 0 �ȉ��Ȃ� 0 �ɂ���
        int targetExperience = Mathf.Max(0, currentExp - amount);
        Debug.Log("���Z���� : " + targetExperience);

        // ���݂̕��탌�x������1���̕��탌�x���̎Z�o
        int minExperienceForCurrentLevel = currentWeaponLevel == 0 ? 0 : weaponLevelDataSO.weaponLevelDataList[currentWeaponLevel - 1].exp;
        Debug.Log("�Œ�o���l : " + minExperienceForCurrentLevel);

        // �o���l�̐����i��L��2�̒l���g���āA�o���l�̉����l�𐧌�����j
        currentExp = Mathf.Max(targetExperience, minExperienceForCurrentLevel);

        Debug.Log(currentExp);

        // ���탌�x���̍X�V
        UpdateWeaponLevel();
    }

    /// <summary>
    /// ���탌�x���̍X�V
    /// </summary>
    private void UpdateWeaponLevel()
    {
        int prevLevel = currentWeaponLevel;
        currentWeaponLevel = -1;

        // �X�N���v�^�u���I�u�W�F�N�g�ɓo�^����Ă��镐�탌�x���f�[�^��������������
        for (int i = weaponLevelDataSO.weaponLevelDataList.Count - 1; i >= 0; i--)
        {
            // ���݂̌o���l���AweaponLevelDataSO.weaponLevelDataList[i] ���̌o���l�����������傫���ꍇ
            if (currentExp >= weaponLevelDataSO.weaponLevelDataList[i].exp)
            {
                // �ő僌�x���̔���ŁA���݂��ő僌�x���Ȃ�
                if (i == weaponLevelDataSO.weaponLevelDataList.Count - 1 && prevLevel >= maxLevel)
                {
                    currentWeaponLevel = prevLevel;
                    break;
                }

                // ���݂̕��탌�x�����AweaponLevelDataSO.weaponLevelDataList[i] ���̕��탌�x���ƈقȂ�ꍇ
                if (currentWeaponLevel != weaponLevelDataSO.weaponLevelDataList[i].level)
                {
                    // ���탌�x���ƕ��탌�x���f�[�^���X�V
                    currentWeaponLevel = weaponLevelDataSO.weaponLevelDataList[i].level;
                    currentWeaponLevelData = weaponLevelDataSO.weaponLevelDataList[i];

                    // UI�Ɍ��݂̕��탌�x����\��
                    playerGunLevel.text = currentWeaponLevel.ToString();

                    // for ���������I������
                    break;
                }
            }
        }
    }
}
