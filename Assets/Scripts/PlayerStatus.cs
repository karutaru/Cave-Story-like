using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    //-------プレイヤーの情報-------
    public int lv; //現在のレベル
    public int hp = 20; //現在の最大HP
    public int currentHP; //現在のHP状況
    public int exp; //現在の経験値

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

    private EnemyController enemyController;
    public AudioClip damageSE;
    [SerializeField]
    private Text playerDamage;


    void Start()
    {
        currentHP = hp;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (enemyController = col.GetComponent<EnemyController>())
        {
            currentHP -= enemyController.atk;
            AudioSource.PlayClipAtPoint(damageSE, transform.position);

            playerDamage.text = currentHP.ToString();
        }
    }

    public void HealHP()
    {
        currentHP = hp;
        playerDamage.text = currentHP.ToString();
    }
}
