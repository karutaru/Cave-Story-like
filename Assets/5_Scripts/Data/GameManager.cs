using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;

/// <summary>
/// �Q�[���̐i�s�Ɋւ��X�N���v�g
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager game; // GameManager.game.�Ƃ����������ŋL�q����B
    // �V�X�e������������
    [Header("�V�X�e��")]
    public InitScript init; // ������

    // �I�u�W�F�N�g������
    [Header("�I�u�W�F�N�g")]
    public Transform playerObj; // �v���C���[�̃I�u�W�F�N�g
    public GameObject object_UI; // UI�摜
    public GameObject object_AIM; // �v���C���[��AIM�摜
    public Transform tran_PlayerCamera; // �v���C���[�p�̃J�����|�C���g
    public Transform tran_EventCamera; // �C�x���g�p�̃J�����|�C���g
    public CinemachineVirtualCamera system_mainCamera; // �J����

    // �G�t�F�N�g������
    [Header("�G�t�F�N�g")]
    public GameObject wallEffectPrefab;
    public List<GameObject> bloodEffectPrefabs;

    // SE������
    [Header("SE")]
    public AudioClip shotHitSE;

    // �X�N���v�g��������
    [Header("�X�N���v�g")]
    public CameraFollowMouse cameraFollowMouse;         // �}�E�X���J�������Ǐ]���鏈��
    public PlayerBodyController playerBodyController;   // �v���C���[�̃X�N���v�g
    public PlayerBodyRotation playerBodyRotation;       // �v���C���[�̈ړ��̐U��ނ��X�N���v�g
    public PlayerHandsScript playerHandsScript;         // �v���C���[�̈ړ��̘r�U��X�N���v�g
    public GunShot gunShot;                             // gunShot�X�N���v�g
    public Knockback_Player knockback_Player;

    // �X�C�b�`����������
    [Header("�X�C�b�`")]
    public bool changeCamera_If = true;

    // �{�^��������������
    [Button]private void UI��AIM�ƃJ�����̃}�E�X�Ǐ]����()
    {
        Title_Disable();
    }

    [Button] private void �v���C���[�J�����ƃC�x���g�J������؂�ւ�()
    {
        ChangeCamera();
    }
    [Button]private void �v���C���[�̓������~�߂�()
    {
        Player_StopMove();
    }
    [Button]private void �v���C���[�̓��������R�ɂ���()
    {
        Player_CanMove();
    }
    [Button]
    private void �e�̎ˌ��֎~()
    {
        Shot_Disable();
    }

    // ------------------------------------------------------------------------------------- �����X�N���v�g���� -------------------------------------------------------------------------------------------

    void Awake()
    {
        if (game == null)
        {
            game = this;
        }
        else if (game != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �^�C�g����ʃ��[�h�Ɉڍs
        //Title_Disable();
    }

    /// <summary>
    /// �^�C�g����ʃ��[�h
    /// </summary>
    public void Title_Disable()
    {
        // �e�I�u�W�F�N�g�̗L��/������Ԃ�؂�ւ��܂��B
        // UI���\��
        object_UI.SetActive(false);
        // AIM�}�[�N���\��
        object_AIM.SetActive(false);
        // �J�����̃}�E�X�ւ̒Ǐ]���֎~
        cameraFollowMouse.enabled = false;
        // �v���C���[�̓������~
        Player_StopMove();
        // �ˌ��֎~
        Shot_Disable();
    }

    public void Title_Enable()
    {
        // �e�I�u�W�F�N�g�̗L��/������Ԃ�؂�ւ��܂��B
        // UI���\��
        object_UI.SetActive(true);
        // AIM�}�[�N���\��
        object_AIM.SetActive(true);
        // �J�����̃}�E�X�ւ̒Ǐ]���֎~
        cameraFollowMouse.enabled = true;
        // �v���C���[�̓������~
        Player_CanMove();
        // �ˌ��֎~
        Shot_Enable();
    }

    public void Player_StopMove()
    {
        playerBodyController.isMove = true;     // �v���C���[�̓�����~
        playerBodyRotation.enabled = false;     // �v���C���[�̐U�������~
        playerHandsScript.enabled = false;      // �v���C���[�̘r�U���~
        playerBodyController.isJump = false;    // �v���C���[�̃W�����v�֎~
    }
    public void Player_CanMove()
    {
        playerBodyController.isMove = false;    // �v���C���[�̓����J�n
        playerBodyRotation.enabled = true;      // �v���C���[�̐U������J�n
        playerHandsScript.enabled = true;       // �v���C���[�̘r�U��J�n
        playerBodyController.isJump = true;     // �v���C���[�̃W�����v����
    }

    public void Shot_Enable() // �ˌ���������
    {
        if (gunShot != null)
        {
            gunShot.isShot = true;
        }
    }

    // �e�����ĂȂ���Ԃɂ���
    public void Shot_Disable() // �ˌ����֎~����
    {
        if (gunShot != null)
        {
            gunShot.isShot = false;
        }
    }

    public void ChangeCamera()
    {
        if (changeCamera_If == true)
        {
            system_mainCamera.Follow = tran_EventCamera; // �C�x���g�J�����ɐ؂�ւ���
            changeCamera_If = false;
        }
        else if (changeCamera_If == false)
        {
            system_mainCamera.Follow = tran_PlayerCamera; // �v���C���[�J�����ɐ؂�ւ���
            changeCamera_If = true;
        }
    }
}
