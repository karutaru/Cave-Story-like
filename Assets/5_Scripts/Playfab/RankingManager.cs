using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Assertions;

public class RankingManager
{
    private async UniTaskVoid Start()
    {

        // ランキング(LeaderBoard)への情報送信(登録)
        UpdatePlayerStatisticsAsync().Forget();
    }

    /// <summary>
    /// ランキング(LeaderBoard)への情報送信(登録)
    /// </summary>
    /// <param name="level"></param>
    /// <param name="travelStage"></param>
    public async UniTask UpdatePlayerStatisticsAsync(int level = 1, int travelStage = 1)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new() {
                new () {
                    StatisticName = "Level",
                    Value = level
                },
                new () {
                    StatisticName = "Travel Stage",
                    Value = travelStage,
                }
            }
        };

        var response = await PlayFabClientAPI.UpdatePlayerStatisticsAsync(request);
        if (response.Error != null)
        {
            Debug.Log(response.Error.GenerateErrorReport());
        }
        Debug.Log("ランキング情報 送信完了");
    }
}