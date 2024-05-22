using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EnemyData
{
    [LabelText("エネミーの画像"), HideLabel]
    [PreviewField(25)]
    public Sprite enemyImage;

    [LabelText("エネミー番号"), HideLabel]
    public int enemyNumber;

    [FoldoutGroup("ステータス")]
    [LabelText("　エネミーの名前"), HideLabel]
    public string enemyName;

    [FoldoutGroup("ステータス")]
    [LabelText("　HP---------------------------"), HideLabel]
    public int hp;

    [FoldoutGroup("ステータス")]
    [LabelText("　攻撃力"), HideLabel]
    public int attackPower;

    [FoldoutGroup("ステータス")]
    [LabelText("　素早さ------------------------"), HideLabel]
    public int speed;

    [FoldoutGroup("ステータス")]
    [LabelText("　攻撃頻度"), HideLabel]
    public int attackSpeed;

    [FoldoutGroup("ステータス")]
    [LabelText("　エネミーのプレハブ"), HideLabel]
    public GameObject enemyPrefab;

    [FoldoutGroup("ステータス")]
    [LabelText("　敵アニメーションのみのプレハブ"), HideLabel] // エネミーアニメーションをMiniBoxに表示する用
    public GameObject enemyAnimPrefab;
}
