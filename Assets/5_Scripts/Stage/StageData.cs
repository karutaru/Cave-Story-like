using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class StageData
{
    [HorizontalGroup("Row 0")]
    [PropertySpace(SpaceBefore = 0, SpaceAfter = 5)]
    [LabelText("�X�e�[�W��"), HideLabel, LabelWidth(100)]
    public string stage_Name;

    [HorizontalGroup("Row 1")]
    [LabelText("�X�e�[�W�ԍ�"), HideLabel]
    public int stage_Number;

    [HorizontalGroup("Row 1")]
    [LabelText("�X�e�[�W�v���n�u"), HideLabel]
    public GameObject stage_Prefab;

    [HorizontalGroup("Row 2")]
    [LabelText("�v���C���[Pos"), HideLabel]
    public Transform stage_PlayerPos;

    [HorizontalGroup("Row 2")]
    [LabelText("�J����Pos"), HideLabel]
    public Transform stage_CameraPos;

    // ���X�g�v�f�̃��x�����J�X�^�}�C�Y����v���p�e�B
    public string ElementLabel
    {
        get { return $"{stage_Name}"; }
    }
}