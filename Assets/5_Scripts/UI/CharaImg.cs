using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;

using System.Linq;  // LINQを使用するために必要

public class CharaImg : MonoBehaviour
{
    public List<Sprite> player_Mouse_Sprites = new List<Sprite>();
    public List<Sprite> player_H_Down_Sprites = new List<Sprite>();
    public List<Sprite> player_Eye_Sprites = new List<Sprite>();
    public List<Sprite> player_H_Up_Sprites = new List<Sprite>();
    public List<Sprite> player_Mayuge_Sprites = new List<Sprite>();

    public Image mouse;
    public Image h_Down;
    public Image eye;
    public Image h_Up;
    public Image mayuge;


    private void Awake()
    {
        var token = this.GetCancellationTokenOnDestroy();
        ListenForKeyPress(token).Forget();  // 非同期タスクの実行
    }

    private async UniTaskVoid ListenForKeyPress(CancellationToken token)
    {
        while (true)
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);  // Fキーの入力を待機
            //await UniTask.Delay()

            ChangeImg();
        }
    }

    //async void Start()
    //{
    //    //GameObjectが破棄された時にキャンセルを飛ばすトークンを作成
    //    var token = this.GetCancellationTokenOnDestroy();
    //    //UniTaskメソッドの引数にCancellationTokenを入れる
    //    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);

    //    ChangeImg();

    //    //GameObjectが破棄された時にキャンセルを飛ばすトークンを作成
    //    token = this.GetCancellationTokenOnDestroy();
    //    //UniTaskメソッドの引数にCancellationTokenを入れる
    //    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);

    //    ChangeImg();

    //    //GameObjectが破棄された時にキャンセルを飛ばすトークンを作成
    //    token = this.GetCancellationTokenOnDestroy();
    //    //UniTaskメソッドの引数にCancellationTokenを入れる
    //    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);

    //    ChangeImg();


    //}



    public void ChangeImg()
    {
        int randomIndex = Random.Range(0, player_Eye_Sprites.Count);  // ランダムなインデックスを取得
        Sprite selectedSprite = player_Eye_Sprites[randomIndex];
        eye.sprite = selectedSprite;

        randomIndex = Random.Range(0, player_H_Down_Sprites.Count);  // ランダムなインデックスを取得
        selectedSprite = player_H_Down_Sprites[randomIndex];
        h_Down.sprite = selectedSprite;

        randomIndex = Random.Range(0, player_Mouse_Sprites.Count);  // ランダムなインデックスを取得
        selectedSprite = player_Mouse_Sprites[randomIndex];
        mouse.sprite = selectedSprite;

        // player_Eye_Spritesの番号が0~2の場合のみ、player_H_Up_Spritesのランダムインデックスを1に固定
        //if (eye.sprite == player_Eye_Sprites[0] || eye.sprite == player_Eye_Sprites[1] || eye.sprite == player_Eye_Sprites[2])
        //{
        //    randomIndex = 1;
        //}
        //else
        //{
        //    randomIndex = Random.Range(0, player_H_Up_Sprites.Count);  // ランダムなインデックスを取得
        //}

        if (player_Eye_Sprites.Take(3).Contains(eye.sprite)) // 上の処理をLinQにした場合
        {
            randomIndex = 1;
        }
        else
        {
            randomIndex = Random.Range(0, player_H_Up_Sprites.Count);
        }


        selectedSprite = player_H_Up_Sprites[randomIndex];
        h_Up.sprite = selectedSprite;

        randomIndex = Random.Range(0, player_Mayuge_Sprites.Count);  // ランダムなインデックスを取得
        selectedSprite = player_Mayuge_Sprites[randomIndex];
        mayuge.sprite = selectedSprite;
    }
}
