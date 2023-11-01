using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

public static class LoginManager
{
    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    static LoginManager()
    {
        // TitleId �ݒ�
        PlayFabSettings.staticSettings.TitleId = "7E7A7";

        Debug.Log($"Title �ݒ� : {PlayFabSettings.staticSettings.TitleId}");
    }

    /// <summary>
    /// ����������
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async UniTaskVoid InitializeAsync()
    {

        Debug.Log("�������J�n");

        // PlayFab �ւ̃��O�C�������ƃ��O�C��
        await PrepareLoginPlayFab();

        Debug.Log("����������");
    }

    /// <summary>
    /// PlayFab �ւ̃��O�C�������ƃ��O�C��
    /// </summary>
    public static async UniTask PrepareLoginPlayFab()
    {
        Debug.Log("���O�C������ �J�n");

        await LoginAndUpdateLocalCacheAsync();

        // ���̃��O�C���̏��(���N�G�X�g)���쐬���Đݒ�
        // �ǂ���ł�����
        // var request = new LoginWithCustomIDRequest {
        //     CustomId = "GettingStartedGuide",
        //     CreateAccount = true
        // };

        // ���̃��O�C���̏��(���N�G�X�g)���쐬���Đݒ�
        // LoginWithCustomIDRequest request = new() {
        //     CustomId = "GettingStartedGuide",  // ���̕��������[�U�[��ID�ɂȂ�܂� 
        //     CreateAccount = true               // �A�J�E���g���쐬����Ă��Ȃ��ꍇ�Atrue �̏ꍇ�͓������O�C�����ăA�J�E���g���쐬����
        // };
        //
        // // PlayFab �փ��O�C���B��񂪊m�F�ł���܂őҋ@
        // var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);
        //
        // // �G���[�̓��e�����āA���O�C���ɐ������Ă��邩�𔻒�(�G���[�n���h�����O)
        // var message = result.Error is null 
        //     ? $"���O�C������! My PlayFabID is { result.Result.PlayFabId }"  // Error �� null �Ȃ��[�G���[�Ȃ�]�Ȃ̂ŁA���O�C������
        //     : result.Error.GenerateErrorReport();                           // Error �� null �ȊO�̏ꍇ�̓G���[���������Ă���̂ŁA���|�[�g�쐬
        //
        // Debug.Log(message);
    }

    public static async UniTask LoginAndUpdateLocalCacheAsync()
    {

        Debug.Log("�������J�n");

        var userId = PlayerPrefsManager.UserId;

        // ���[�U�[ID �̎擾�����݂āA�擾�ł��Ȃ��ꍇ�ɂ͓����ŐV�K�쐬����
        var loginResult = string.IsNullOrEmpty(userId)
            ? await CreateNewUserAsync()
            : await LoadUserAsync(userId);

        Debug.Log(loginResult);


        //Debug �����L���O���M
         RankingManager rankingManager = new();
        await rankingManager.UpdatePlayerStatisticsAsync(2, 3);

        //await rankingManager.GetLeaderboardAsync("Level");
        //await rankingManager.GetLeaderboardAsync("HighScore");
    }

    private static async UniTask<LoginResult> CreateNewUserAsync()
    {

        Debug.Log("���[�U�[�f�[�^�Ȃ��B�V�K���[�U�[�쐬");

        while (true)
        {

            // UserId �̍̔�
            var newUserId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            // ���O�C�����N�G�X�g�̍쐬(�ȉ��̏����́A���܂܂� PrepareLoginPlayPab ���\�b�h���ɏ����Ă��������̂��C�����ċL�q)
            var request = new LoginWithCustomIDRequest
            {
                CustomId = newUserId,
                CreateAccount = true
            };

            // PlayFab �Ƀ��O�C��
            var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

            // �G���[�n���h�����O
            if (response.Error != null)
            {
                Debug.Log($"Error : {response.Error.GenerateErrorReport()}");
            }

            // ������ LastLoginTime �ɒl�������Ă���ꍇ�ɂ́A�̔Ԃ��� ID ���������[�U�[�Əd�����Ă���̂Ń��g���C����
            if (response.Result.LastLoginTime.HasValue)
            {
                continue;
            }

            // PlayerPrefs �� UserId ���L�^����
            PlayerPrefsManager.UserId = newUserId;

            return response.Result;
        }
    }

    /// <summary>
    /// ���O�C�����ă��[�U�[�f�[�^�����[�h
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private static async UniTask<LoginResult> LoadUserAsync(string userId)
    {

        Debug.Log("���[�U�[�f�[�^����B���O�C���J�n");

        // ���O�C�����N�G�X�g�̍쐬
        var request = new LoginWithCustomIDRequest
        {
            CustomId = userId,
            CreateAccount = false // <= �A�J�E���g�̏㏑�������͍s��Ȃ��悤�ɂ���
        };

        // PlayFab �Ƀ��O�C��
        var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        // �G���[�n���h�����O
        if (response.Error != null)
        {
            Debug.Log("Error");

            // TODO response.Error �ɂ̓G���[�̎�ނ��l�Ƃ��ē����Ă���
            // ���̃G���[�ɑΉ����������� switch ���ȂǂŋL�q���ĕ����̃G���[�ɑΉ��ł���悤�ɂ���
        }

        // �G���[�̓��e�����ăn���h�����O���s���A���O�C���ɐ������Ă��邩�𔻒�
        var message = response.Error is null
            ? $"Login success! My PlayFabID is {response.Result.PlayFabId}"
            : response.Error.GenerateErrorReport();

        Debug.Log(message);

        return response.Result;
    }
}