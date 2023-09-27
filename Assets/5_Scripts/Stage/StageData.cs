using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]

public class StageData
{
    [LabelText("　■■■ステージ名■■■"), HideLabel]
    public string enemyName;

    [LabelText("　ステージ番号"), HideLabel]
    public int enemyNumber;

    [LabelText("　プレハブ"), HideLabel]
    public GameObject stagePrefab;
}
