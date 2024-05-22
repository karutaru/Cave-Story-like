using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerStatus : MonoBehaviour
{
    //-------プレイヤーの情報-------
    public int level = 1; //現在のレベル
    public int hp_Max = 20; //現在の最大HP
    public int hp_Current = 20; //現在のHP状況
    public int exp = 0; //累計の経験値
    public int exp_Current = 0; //現在の経験値
    public float exp_Next = 8; //次の要求レベルアップEXP

    //-------武器の情報-------
    public int gunAt; //現在の武器の攻撃力
    public int gunMaxAmmo; //現在の武器の最大装弾数
    public int gunShotSpeed; //現在の武器の撃てる速さ
    public int gunShotRange; //現在の武器の射程距離
    public int gunReloadSpeed; //現在の武器のリロード速度

    //-------装備の情報-------
    public bool map; //マップを持っているか
    public bool jetBoots; //ジャットブーツを持っているか

    //---------------------------ここまで---------------------------

    public int baseExp = 8; //初期の必要な経験値

    private EnemyBase enemyBase;
    public HeartsHealthDisplay heartsHealthDisplay;

    [SerializeField]
    private Text player_CurrentHp;
    [SerializeField]
    private Text player_MaxHp;
    [SerializeField]
    private Text player_Level;
    [SerializeField]
    private Text player_CurrentExp;
    [SerializeField]
    private Text player_NextExp;
    [SerializeField]
    private Image hpBarImage;
    [SerializeField]
    private Image expBarImage;


    void Start()
    {
        hp_Current = hp_Max;
        player_MaxHp.text = hp_Max.ToString();
        player_CurrentHp.text = hp_Current.ToString();
    }

    public void HitEnemy(Collider2D col)
    {
        if (enemyBase = col.GetComponent<EnemyBase>())
        {
            hp_Current -= enemyBase.attack;

            heartsHealthDisplay.TakeDamage(enemyBase.attack);
        }
    }

    public void HealHP()
    {
        //hp_Current = hp_Max;
        //player_CurrentHp.text = hp_Current.ToString();
    }

    public void AddExp(int amount)
    {
        exp += amount;
        exp_Current += amount;

        if (exp_Next <= exp_Current)
        {
            LevelUp(level);
        }
        player_CurrentExp.text = exp_Current.ToString();
    }

    public void LevelUp(int amount)
    {
        level = amount + 1;
        player_Level.text = level.ToString();

        // レベルに基づいて必要経験値を計算
        exp_Next = Mathf.RoundToInt(baseExp * Mathf.Pow(1.2f, level - 1));

        player_NextExp.text = exp_Next.ToString();
        Debug.Log(exp_Next);
        exp_Current = 0;
    }
}
