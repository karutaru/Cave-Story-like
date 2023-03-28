using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager instance;
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Image face;
    [SerializeField]
    private Text talkText;
    [SerializeField]
    private float messageSpeed = 0.05f;
    [SerializeField]
    private AudioClip player_TalkSE;
    AudioSource audioSource;
    private bool isDisplay;
    private string beforeText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }



        audioSource = GetComponent<AudioSource>();
    }

    private static readonly string[] INVALID_CHARS = {
        " ", "　", "!", "?", "！", "？", "\"", "\'", "\\",
        ".", ",", "、", "。", "…", "・", "（", "）", "(", ")"
    };


    public void PrepareDisplayMessage(string message, EventBase eventBase)
    {
        StartCoroutine(DisplayMessage(message, eventBase));
        beforeText = message;
    }

    private IEnumerator DisplayMessage(string message, EventBase eventBase)
    {
                textBox.enabled = true;
                face.enabled = true;
                talkText.enabled = true;

                talkText.DOText(message, message.Length * messageSpeed).SetEase(Ease.Linear).OnUpdate(() =>
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
                    audioSource.PlayOneShot(player_TalkSE);
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
            eventBase.isEventPlay = false;
    }

    private void ExitDisplay()
    {
        textBox.enabled = false;
        face.enabled = false;
        talkText.text = string.Empty;
        talkText.enabled = false;
        isDisplay = false;
    }
}
