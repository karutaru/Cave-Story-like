using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Create StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, ShowIndexLabels = true, CustomAddFunction = "CreateNewStageData", ListElementLabelName = "ElementLabel")]
    public List<StageData> stageDataList = new List<StageData>();

    // 新しいStageDataを作成するカスタム関数
    private StageData CreateNewStageData()
    {
        return new StageData
        {
            stage_Name = "",
            stage_Number = stageDataList.Count,
            stage_Prefab = null,
            stage_PlayerPos = null,
            stage_CameraPos = null
        };
    }
}