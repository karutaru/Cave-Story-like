using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]

public class StageData
{
    [LabelText("�@�������X�e�[�W��������"), HideLabel]
    public string enemyName;

    [LabelText("�@�X�e�[�W�ԍ�"), HideLabel]
    public int enemyNumber;

    [LabelText("�@�v���n�u"), HideLabel]
    public GameObject stagePrefab;
}
