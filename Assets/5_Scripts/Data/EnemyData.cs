using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EnemyData
{
    [LabelText("�G�l�~�[�̉摜"), HideLabel]
    [PreviewField(25)]
    public Sprite enemyImage;

    [LabelText("�G�l�~�[�ԍ�"), HideLabel]
    public int enemyNumber;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@�G�l�~�[�̖��O"), HideLabel]
    public string enemyName;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@HP---------------------------"), HideLabel]
    public int hp;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@�U����"), HideLabel]
    public int attackPower;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@�f����------------------------"), HideLabel]
    public int speed;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@�U���p�x"), HideLabel]
    public int attackSpeed;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@�G�l�~�[�̃v���n�u"), HideLabel]
    public GameObject enemyPrefab;

    [FoldoutGroup("�X�e�[�^�X")]
    [LabelText("�@�G�A�j���[�V�����݂̂̃v���n�u"), HideLabel] // �G�l�~�[�A�j���[�V������MiniBox�ɕ\������p
    public GameObject enemyAnimPrefab;
}
