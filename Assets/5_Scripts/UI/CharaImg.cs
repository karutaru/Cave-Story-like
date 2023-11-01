using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;

using System.Linq;  // LINQ���g�p���邽�߂ɕK�v

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
        ListenForKeyPress(token).Forget();  // �񓯊��^�X�N�̎��s
    }

    private async UniTaskVoid ListenForKeyPress(CancellationToken token)
    {
        while (true)
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);  // F�L�[�̓��͂�ҋ@
            //await UniTask.Delay()

            ChangeImg();
        }
    }

    //async void Start()
    //{
    //    //GameObject���j�����ꂽ���ɃL�����Z�����΂��g�[�N�����쐬
    //    var token = this.GetCancellationTokenOnDestroy();
    //    //UniTask���\�b�h�̈�����CancellationToken������
    //    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);

    //    ChangeImg();

    //    //GameObject���j�����ꂽ���ɃL�����Z�����΂��g�[�N�����쐬
    //    token = this.GetCancellationTokenOnDestroy();
    //    //UniTask���\�b�h�̈�����CancellationToken������
    //    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);

    //    ChangeImg();

    //    //GameObject���j�����ꂽ���ɃL�����Z�����΂��g�[�N�����쐬
    //    token = this.GetCancellationTokenOnDestroy();
    //    //UniTask���\�b�h�̈�����CancellationToken������
    //    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.F), cancellationToken: token);

    //    ChangeImg();


    //}



    public void ChangeImg()
    {
        int randomIndex = Random.Range(0, player_Eye_Sprites.Count);  // �����_���ȃC���f�b�N�X���擾
        Sprite selectedSprite = player_Eye_Sprites[randomIndex];
        eye.sprite = selectedSprite;

        randomIndex = Random.Range(0, player_H_Down_Sprites.Count);  // �����_���ȃC���f�b�N�X���擾
        selectedSprite = player_H_Down_Sprites[randomIndex];
        h_Down.sprite = selectedSprite;

        randomIndex = Random.Range(0, player_Mouse_Sprites.Count);  // �����_���ȃC���f�b�N�X���擾
        selectedSprite = player_Mouse_Sprites[randomIndex];
        mouse.sprite = selectedSprite;

        // player_Eye_Sprites�̔ԍ���0~2�̏ꍇ�̂݁Aplayer_H_Up_Sprites�̃����_���C���f�b�N�X��1�ɌŒ�
        //if (eye.sprite == player_Eye_Sprites[0] || eye.sprite == player_Eye_Sprites[1] || eye.sprite == player_Eye_Sprites[2])
        //{
        //    randomIndex = 1;
        //}
        //else
        //{
        //    randomIndex = Random.Range(0, player_H_Up_Sprites.Count);  // �����_���ȃC���f�b�N�X���擾
        //}

        if (player_Eye_Sprites.Take(3).Contains(eye.sprite)) // ��̏�����LinQ�ɂ����ꍇ
        {
            randomIndex = 1;
        }
        else
        {
            randomIndex = Random.Range(0, player_H_Up_Sprites.Count);
        }


        selectedSprite = player_H_Up_Sprites[randomIndex];
        h_Up.sprite = selectedSprite;

        randomIndex = Random.Range(0, player_Mayuge_Sprites.Count);  // �����_���ȃC���f�b�N�X���擾
        selectedSprite = player_Mayuge_Sprites[randomIndex];
        mayuge.sprite = selectedSprite;
    }
}
