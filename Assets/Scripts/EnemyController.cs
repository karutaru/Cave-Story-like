using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHP; //エネミーの最大HP
    public int currentHP; //エネミーの現在のHP
    public int atk = 2; //エネミーの攻撃力
    public int exp; //エネミーの経験値
    private int takeDamage;
    public BulletController bulletController;
    public GameObject ExplosionFirstPrefab;
    public GameObject ExplosionPrefab;
    public AudioClip explosionSE;

    [SerializeField]
    private GameObject damagePopupPrefab;


    void Start()
    {
        currentHP = maxHP;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<BulletController>())
        {
            //takeDamage = bulletController.weaponDamage;
            // currentHP -= bulletController.weaponDamage; //ダメージの変数
            currentHP -= 2;

            // ダメージポップアップを表示
            // GameObject damagePopupInstance = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity, transform);
            // DamagePopup damagePopup = damagePopupInstance.GetComponent<DamagePopup>();
            // damagePopup.SetDamage(takeDamage);

            if (currentHP <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSE, transform.position);

                GameObject ExplosionFirstEffect = Instantiate(ExplosionFirstPrefab, transform.position, Quaternion.identity);
                GameObject ExplosionEffect = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                //エフェクトを消す
                Destroy(ExplosionFirstEffect, 0.4f);
                Destroy(ExplosionEffect, 1f);

                Destroy(this.gameObject);
            }
        }
    }
}
