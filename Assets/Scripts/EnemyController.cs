using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHP; //エネミーの最大HP
    public int currentHP; //エネミーの現在のHP


    void Start()
    {
        currentHP = maxHP;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Shell"))
        {
            currentHP -= 2; //ダメージの変数をプレイヤー情報から持ってくる予定地

            if (currentHP <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
