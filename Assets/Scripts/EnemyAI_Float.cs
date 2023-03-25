using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class EnemyAI_Float : MonoBehaviour
{
    private bool isUp = false;
    private int timer;
    private int shotTimer;
    private float direction; //向いている方向
    private float lookDirection;
    public bool discoverPlayer;
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    public MMF_Player MMFPlayer_Scale;
    public MMF_Player MMFPlayer_Shot;
    public AudioClip shotSE;
    private Animator anim;
    private Tweener tweener;
    public Transform player;
    [SerializeField, Header("Linecast用 プレイヤー判定レイヤー")]
    private LayerMask groundLayer;


    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        Attack();
        FloatEnemy();
        LookPlayer();
    }


    void Attack()
    {
        discoverPlayer = Physics2D.Linecast(transform.position - transform.right * 9f, transform.position - transform.right * 0.1f, groundLayer);
        Debug.DrawLine(transform.position - transform.right * 9f, transform.position - transform.right * 0.1f, Color.red, 0.2f);

        if (discoverPlayer == true && shotTimer == 0)
        {
            tweener.Pause(); //一時停止
            anim.Play("Charge");
            StartCoroutine("Shot");

            shotTimer ++;
        }
    }

    IEnumerator Shot()
    {
 
        yield return new WaitForSeconds(1); //1秒停止

        anim.Play("Shot");
        
        //弾エフェクトを実体化する
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
        bullet.transform.DOLocalMoveX(bullet.transform.position.x + 20f * direction, 2f).SetEase(Ease.InExpo).SetLink(bullet);
        //弾を1秒後に消す
        Destroy(bullet, 2f);

        MMFPlayer_Shot.Initialization();
        MMFPlayer_Shot.PlayFeedbacks(); //震える
        
        yield return new WaitForSeconds(0.05f); //0.05秒停止
        //弾2エフェクトを実体化する
        GameObject bullet2 = Instantiate(bulletPrefab2, this.transform.position, Quaternion.identity);
        bullet2.transform.DOLocalMoveX(bullet2.transform.position.x + 20f * direction, 2f).SetEase(Ease.InExpo).SetLink(bullet2);
        //弾2を1秒後に消す
        Destroy(bullet2, 2f);

        yield return new WaitForSeconds(0.7f); //0.7秒停止

        anim.Play("Idle");

        tweener.Play(); //再開

        yield return new WaitForSeconds(0.1f); //0.1秒停止

        discoverPlayer = false;
        shotTimer = 0;
    }


    //エネミーを浮かせてアニメーション
    void FloatEnemy()
    {
        if (isUp == true && timer == 0)
        {
            MMFPlayer_Scale.Initialization();
            MMFPlayer_Scale.PlayFeedbacks();
            //現在の座標からY-3の座標へ3秒で移動する
            tweener = this.transform.DOLocalMoveY(this.transform.position.y - 3f, 3f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                isUp = false;
                timer = 0;
            }).SetLink(gameObject);

        } else if (isUp == false && timer == 0) {
            MMFPlayer_Scale.Initialization();
            MMFPlayer_Scale.PlayFeedbacks();
            //現在の座標からY+3の座標へ3秒で移動する
            tweener = this.transform.DOLocalMoveY(this.transform.position.y + 3f, 3f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                isUp = true;
                timer = 0;
            }).SetLink(gameObject);
        }

        timer ++;
    }

    //向きをプレイヤーの方向へ変える
    void LookPlayer()
    {
        lookDirection = player.transform.position.x - this.gameObject.transform.position.x;

        if (lookDirection > 0)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            direction = 1;
        }
        if (lookDirection < 0)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            direction = -1;
        }
    }
}

