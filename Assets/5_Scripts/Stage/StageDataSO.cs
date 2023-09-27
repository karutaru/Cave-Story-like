using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StageDataSO", menuName = "Create StageDataSO")]

public class StageDataSO : ScriptableObject
{
    public List<StageData> stageDataList = new List<StageData>();
}
