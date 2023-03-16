using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    public BulletCountController bulletCountController;
    public float gunReloadTime = 100f; //武器のリロードの長さ
    public float needReloadTime;
    public int maxbullets; //最大弾数
    public float timer; //時間
    public float reloadTime; //リロード中の時間
    public bool isReloading; //リロード中か
    [SerializeField] 
    GameObject reloadObject; //同列のリロードオブジェクト
    [SerializeField]
    PlayerController playerController;

    void Start()
    {
        this.gameObject.transform.localPosition = new Vector2 (0.95f, 0f);
    }

    private void Update()
    {
        

        if (isReloading == true)
        {
            timer += Time.deltaTime; //時間をはかる

            if (this.gameObject.transform.localPosition.x <= -0.95f) // timerが-1.9以下なら
            {
                //タイマーをリセット
                timer = 0f;
                //位置を元に戻す
                this.gameObject.transform.localPosition = new Vector2 (0.95f, 1.4f);

                isReloading = false;

                reloadObject.SetActive (false);
                this.gameObject.SetActive (false);

            } else {
                this.gameObject.transform.localPosition = new Vector2 (0.95f + timer * -1.9f / gunReloadTime, 1.4f);
        }
        }
    }


    public void ReloadBullets(int amount)
    {
        reloadObject.SetActive (true);
        this.gameObject.SetActive (true);

        // maxbullets = amount; //弾の最大数

        // reloadTime = -1.9f / gunReloadTime; //時間をはかる

        isReloading = true;

        // int i = 0;

        // while (i == maxbullets) {
        //     Debug.Log(i);
        //     i++;
        // }
    }
}
