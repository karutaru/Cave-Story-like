using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //-------プレイヤーの情報-------
    public int lv; //現在のレベル
    public int hp; //現在の最大HP
    public int exp; //現在の経験値

    //-------武器の情報-------
    public int gunAt; //現在の武器の攻撃力
    public int gunMaxAmmo; //現在の武器の最大装弾数
    public int gunShotSpeed; //現在の武器の撃てる速さ
    public int gunShotRange; //現在の武器の射程距離
    public int gunReloadSpeed; //現在の武器のリロード速度
    public int gunAmount; //現在の武器の弾の増加数

    //-------装備の情報-------
    public bool map; //マップを持っているか
    public bool jetBoots; //ジャットブーツを持っているか

    //---------------------------ここまで---------------------------


    void Start()
    {
        gunMaxAmmo = 5; //仮の数字。5発
    }
}
