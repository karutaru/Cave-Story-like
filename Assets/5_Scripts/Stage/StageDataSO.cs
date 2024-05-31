using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Create StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = true, CustomAddFunction = "CreateNewStageData", ListElementLabelName = "ElementLabel")]
    public List<StageData> stageDataList = new List<StageData>();

    // �V����StageData���쐬����J�X�^���֐�
    private StageData CreateNewStageData()
    {
        return new StageData
        {
            stage_Name = "New Stage",
            stage_Number = stageDataList.Count + 1,
            stage_Prefab = null,
            stage_PlayerPos = null,
            stage_CameraPos = null
        };
    }
}