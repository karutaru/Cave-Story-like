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

    [SerializeField]
    private Text playerGunLevel;
}
