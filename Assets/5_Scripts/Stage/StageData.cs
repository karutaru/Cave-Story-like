using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class StageData
{
    [HorizontalGroup("Row 0")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [LabelText("ステージ名"), HideLabel, LabelWidth(100)]
    public string stage_Name;

    [HorizontalGroup("Row 1")]
    [LabelText("ステージ番号"), HideLabel]
    public int stage_Number;

    [HorizontalGroup("Row 1")]
    [LabelText("ステージプレハブ"), HideLabel]
    public GameObject stage_Prefab;

    [HorizontalGroup("Row 2")]
    [LabelText("プレイヤーPos"), HideLabel]
    public Transform stage_PlayerPos;

    [HorizontalGroup("Row 2")]
    [LabelText("カメラPos"), HideLabel]
    public Transform stage_CameraPos;

    // リスト要素のラベルをカスタマイズするプロパティ
    public string ElementLabel
    {
        get { return $"{stage_Name}"; }
    }
}