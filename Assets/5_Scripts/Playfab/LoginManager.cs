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
    /// コンストラクタ
    /// </summary>
    static LoginManager()
    {
        // TitleId 設定
        PlayFabSettings.staticSettings.TitleId = "7E7A7";

        Debug.Log($"Title 設定 : {PlayFabSettings.staticSettings.TitleId}");
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async UniTaskVoid InitializeAsync()
    {

        Debug.Log("初期化開始");

        // PlayFab へのログイン準備とログイン
        await PrepareLoginPlayFab();

        Debug.Log("初期化完了");
    }

    /// <summary>
    /// PlayFab へのログイン準備とログイン
    /// </summary>
    public static async UniTask PrepareLoginPlayFab()
    {
        Debug.Log("ログイン準備 開始");

        await LoginAndUpdateLocalCacheAsync();

        // 仮のログインの情報(リクエスト)を作成して設定
        // どちらでもいい
        // var request = new LoginWithCustomIDRequest {
        //     CustomId = "GettingStartedGuide",
        //     CreateAccount = true
        // };

        // 仮のログインの情報(リクエスト)を作成して設定
        // LoginWithCustomIDRequest request = new() {
        //     CustomId = "GettingStartedGuide",  // この部分がユーザーのIDになります 
        //     CreateAccount = true               // アカウントが作成されていない場合、true の場合は匿名ログインしてアカウントを作成する
        // };
        //
        // // PlayFab へログイン。情報が確認できるまで待機
        // var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);
        //
        // // エラーの内容を見て、ログインに成功しているかを判定(エラーハンドリング)
        // var message = result.Error is null 
        //     ? $"ログイン成功! My PlayFabID is { result.Result.PlayFabId }"  // Error が null ならば[エラーなし]なので、ログイン成功
        //     : result.Error.GenerateErrorReport();                           // Error が null 以外の場合はエラーが発生しているので、レポート作成
        //
        // Debug.Log(message);
    }

    public static async UniTask LoginAndUpdateLocalCacheAsync()
    {

        Debug.Log("初期化開始");

        var userId = PlayerPrefsManager.UserId;

        // ユーザーID の取得を試みて、取得できない場合には匿名で新規作成する
        var loginResult = string.IsNullOrEmpty(userId)
            ? await CreateNewUserAsync()
            : await LoadUserAsync(userId);

        Debug.Log(loginResult);


        //Debug ランキング送信
         RankingManager rankingManager = new();
        await rankingManager.UpdatePlayerStatisticsAsync(2, 3);

        //await rankingManager.GetLeaderboardAsync("Level");
        //await rankingManager.GetLeaderboardAsync("HighScore");
    }

    private static async UniTask<LoginResult> CreateNewUserAsync()
    {

        Debug.Log("ユーザーデータなし。新規ユーザー作成");

        while (true)
        {

            // UserId の採番
            var newUserId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            // ログインリクエストの作成(以下の処理は、いままで PrepareLoginPlayPab メソッド内に書いてあったものを修正して記述)
            var request = new LoginWithCustomIDRequest
            {
                CustomId = newUserId,
                CreateAccount = true
            };

            // PlayFab にログイン
            var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

            // エラーハンドリング
            if (response.Error != null)
            {
                Debug.Log($"Error : {response.Error.GenerateErrorReport()}");
            }

            // もしも LastLoginTime に値が入っている場合には、採番した ID が既存ユーザーと重複しているのでリトライする
            if (response.Result.LastLoginTime.HasValue)
            {
                continue;
            }

            // PlayerPrefs に UserId を記録する
            PlayerPrefsManager.UserId = newUserId;

            return response.Result;
        }
    }

    /// <summary>
    /// ログインしてユーザーデータをロード
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private static async UniTask<LoginResult> LoadUserAsync(string userId)
    {

        Debug.Log("ユーザーデータあり。ログイン開始");

        // ログインリクエストの作成
        var request = new LoginWithCustomIDRequest
        {
            CustomId = userId,
            CreateAccount = false // <= アカウントの上書き処理は行わないようにする
        };

        // PlayFab にログイン
        var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

        // エラーハンドリング
        if (response.Error != null)
        {
            Debug.Log("Error");

            // TODO response.Error にはエラーの種類が値として入っている
            // そのエラーに対応した処理を switch 文などで記述して複数のエラーに対応できるようにする
        }

        // エラーの内容を見てハンドリングを行い、ログインに成功しているかを判定
        var message = response.Error is null
            ? $"Login success! My PlayFabID is {response.Result.PlayFabId}"
            : response.Error.GenerateErrorReport();

        Debug.Log(message);

        return response.Result;
    }
}