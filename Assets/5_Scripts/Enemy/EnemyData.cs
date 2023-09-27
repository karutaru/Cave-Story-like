using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EnemyData
{
    [LabelText("★エネミーの画像"), HideLabel]
    [PreviewField(40)]
    public Sprite enemyImage;

    [LabelText("　■■■エネミーの名前■■■"), HideLabel]
    public string enemyName;

    [LabelText("　エネミー番号"), HideLabel]
    public int enemyNumber;

    [LabelText("　HP---------------------------"), HideLabel]
    public int hp;

    [LabelText("　攻撃力"), HideLabel]
    public int attackPower;

    [LabelText("　素早さ------------------------"), HideLabel]
    public int speed;

    [LabelText("　攻撃頻度"), HideLabel]
    public int attackSpeed;

    [LabelText("　エネミーのプレハブ"), HideLabel]
    public GameObject enemyPrefab;

    [LabelText("　敵アニメーションのみのプレハブ"), HideLabel] // エネミーアニメーションをMiniBoxに表示する用
    public GameObject enemyAnimPrefab;
}
