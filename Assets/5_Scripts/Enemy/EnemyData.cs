using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EnemyData
{
    [LabelText("���G�l�~�[�̉摜"), HideLabel]
    [PreviewField(40)]
    public Sprite enemyImage;

    [LabelText("�@�������G�l�~�[�̖��O������"), HideLabel]
    public string enemyName;

    [LabelText("�@�G�l�~�[�ԍ�"), HideLabel]
    public int enemyNumber;

    [LabelText("�@HP---------------------------"), HideLabel]
    public int hp;

    [LabelText("�@�U����"), HideLabel]
    public int attackPower;

    [LabelText("�@�f����------------------------"), HideLabel]
    public int speed;

    [LabelText("�@�U���p�x"), HideLabel]
    public int attackSpeed;

    [LabelText("�@�G�l�~�[�̃v���n�u"), HideLabel]
    public GameObject enemyPrefab;

    [LabelText("�@�G�A�j���[�V�����݂̂̃v���n�u"), HideLabel] // �G�l�~�[�A�j���[�V������MiniBox�ɕ\������p
    public GameObject enemyAnimPrefab;
}
