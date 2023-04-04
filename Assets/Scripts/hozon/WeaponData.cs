using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public enum WeaponType {
    ディーバ, //初期武器
    ラグーン, //サブマシンガン。弾は水色で、終点が白いため波のように見える
    ジャンボリー, //カラフルで激しい弾幕を発射する
    モノリス, //上から黒い板を落とす
    ラメント, //撃つと緩やかにイーズで停止し、周囲にダメージのある波動を放った後、さらに弾をランダム前方に生成する
    エレジー, //撃つとランダムな間隔で途中分裂する。敵から遠くで撃つと弾が増え威力が増す
    エニグマ, //
    ドッペルゲンガー, //分身を召喚する
    メメント, //
    モノクローム, //
    ナイトメア, //黒い稲妻を発射する
    ノヴァ, //
    オーバードーズ //
    }
}
