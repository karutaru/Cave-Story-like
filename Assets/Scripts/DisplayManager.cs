using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using JetBrains.Annotations;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager instance;

    [SerializeField]
    private Image textBox;                          // テキストボックスの画像
    [SerializeField]
    private Image FaceIcon;                         // 表情を差し込む用の画像

    [SerializeField]
    private Sprite[] playerFaceSprites;             // プレイヤーの表情スプライトを入れる変数
    [SerializeField]
    private Sprite[] doctorFaceSprites;             // ドクターの表情スプライトを入れる変数

    private Dictionary<EventMessage.FACE, Sprite> playerFaceSpriteDict;         // プレイヤーの表情スプライトの辞書
    private Dictionary<EventMessage.FACE, Sprite> doctorFaceSpriteDict;         // ドクターの表情スプライトの辞書

    public EventMessage.FACE currentPlayerFace;     // 現在のプレイヤーの表情
    public EventMessage.FACE currentDoctorFace;     // 現在のドクターの表情
    private bool playerTurn;                        // プレイヤーが話し始めた時にtrueになる
    private bool doctorTurn;                        // ドクターが話し始めた時にtrueになる

    [SerializeField]
    private Text talkText;                          // 会話を表示するテキスト
    [SerializeField]
    private float messageSpeed = 0.05f;             // 文字送りのスピード
    [SerializeField]
    private AudioClip player_TalkSE;                // プレイヤーの声のSE

    AudioSource audioSource;
    private bool isDisplay;
    private string beforeText;

    //-----------------------------------------ここまで------------------------------------------------------------------

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // Dictionaryの初期化
        playerFaceSpriteDict = new Dictionary<EventMessage.FACE, Sprite>()
        {
            { EventMessage.FACE.なし, playerFaceSprites[0] },
            { EventMessage.FACE.普通, playerFaceSprites[1] },
            { EventMessage.FACE.笑顔, playerFaceSprites[2] },
            { EventMessage.FACE.怒り顔, playerFaceSprites[3] },
            { EventMessage.FACE.悲しい顔, playerFaceSprites[4] },
            { EventMessage.FACE.楽しい顔, playerFaceSprites[5] }
        };
        doctorFaceSpriteDict = new Dictionary<EventMessage.FACE, Sprite>()
        {
            { EventMessage.FACE.なし, doctorFaceSprites[0] },
            { EventMessage.FACE.普通, doctorFaceSprites[1] },
            { EventMessage.FACE.笑顔, doctorFaceSprites[2] },
            { EventMessage.FACE.怒り顔, doctorFaceSprites[3] },
            { EventMessage.FACE.悲しい顔, doctorFaceSprites[4] },
            { EventMessage.FACE.楽しい顔, doctorFaceSprites[5] }
        };
        audioSource = GetComponent<AudioSource>();
    }

    // 弾く単語
    private static readonly string[] INVALID_CHARS = {
        " ", "　", "!", "?", "！", "？", "\"", "\'", "\\",
        ".", ",", "、", "。", "…", "・", "（", "）", "(", ")"
    };

    /// <summary>
    /// メッセージを表示する
    /// </summary>
    /// <param name="talks"></param>
    /// <param name="eventBase"></param>
    /// <param name="eventMessage"></param>
    /// <param name="playerFaceName"></param>
    /// <param name="doctorFaceName"></param>
    public void PrepareDisplayMessage(string talks, EventBase eventBase, EventMessage eventMessage, EventMessage.FACE playerFaceName, EventMessage.FACE doctorFaceName)
    {
        // 表情の数値を更新
        currentPlayerFace = playerFaceName;
        currentDoctorFace = doctorFaceName;

        // currentPlayerFaceが[0]（なし）でなければ、プレイヤーの表情スプライトを設定
        if (playerFaceSpriteDict[currentPlayerFace] != playerFaceSpriteDict[0])
        {
            // アイコンのスプライトを指定した表情に変更
            FaceIcon.sprite = playerFaceSpriteDict[currentPlayerFace];

            // プレイヤーが会話をし始めるターン
            playerTurn = true;
            doctorTurn = false;
        }
        // currentDoctorFaceが[0]（なし）でなければ、ドクターの表情スプライトを設定
        if (doctorFaceSpriteDict[currentDoctorFace] != doctorFaceSpriteDict[0])
        {
            // アイコンのスプライトを指定した表情に変更
            FaceIcon.sprite = doctorFaceSpriteDict[currentDoctorFace];

            // ドクターが会話をし始めるターン
            doctorTurn = true;
            playerTurn = false;
        }

        StartCoroutine(DisplayMessage(talks, eventBase, eventMessage));
        beforeText = talks;
    }

    private IEnumerator DisplayMessage(string talks, EventBase eventBase, EventMessage eventMessage)
    {
        // 会話用のUIを表示
        textBox.enabled = true;
        talkText.enabled = true;
        FaceIcon.enabled = true;

        // テキストを1文字ずつ文字送りする
        talkText.DOText(talks, talks.Length * messageSpeed).SetEase(Ease.Linear).OnUpdate(() =>
        {
        var currentText = talkText.text;
        if (beforeText == currentText)
        {
            return;
        }
        var lastIndex = currentText.Length - 1;
        if (lastIndex >= 0)
        {
            var newChar = currentText[lastIndex].ToString();
            if (!INVALID_CHARS.Contains(newChar))
            {
                if (playerTurn)
                {
                    audioSource.PlayOneShot(player_TalkSE);
                }
                if (doctorTurn)
                {
                    // TODO ここにドクターの声を入れる
                    audioSource.PlayOneShot(player_TalkSE);
                }
            }
        }

        //次のチェック用にテキスト更新
        beforeText = currentText;

        }).OnComplete(() =>
        {
            isDisplay = true;
        });
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) && isDisplay == true);
        ExitDisplay();
        //eventBase.isEventPlay = false;

        eventMessage.FinishEvent();
    }

    private void ExitDisplay()
    {
        textBox.enabled = false;
        FaceIcon.enabled = false;
        talkText.text = string.Empty;
        talkText.enabled = false;
        isDisplay = false;
    }
}
