using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class SystemTextManager : MonoBehaviour
{
    public static SystemTextManager instanceSystemText;
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Text systemText;
    [SerializeField]
    private float messageSpeed = 0.05f;
    [SerializeField]
    private AudioClip systemSE;
    AudioSource audioSource;
    private bool isDisplay;
    private string beforeText;
    public PlayerStatus playerStatus;



    void Awake()
    {
        if (instanceSystemText == null)
        {
            instanceSystemText = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        
        audioSource = GetComponent<AudioSource>();
    }

    private static readonly string[] INVALID_CHARS = {
        " ", "　", "!", "?", "！", "？", "\"", "\'", "\\",
        ".", ",", "、", "。", "…", "・"
    };

    public void PrepareSystemMessage(string systemMessage, EventBase eventBase)
    {
        StartCoroutine(SyestemMessage(systemMessage, eventBase));
        beforeText = systemMessage;
    }

    private IEnumerator SyestemMessage(string systemMessage, EventBase eventBase)
    {

        textBox.enabled = true;
        systemText.enabled = true;

        systemText.DOText(systemMessage, systemMessage.Length * messageSpeed).SetEase(Ease.Linear).OnUpdate(() =>
        {
            var currentText = systemText.text;
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
                    audioSource.PlayOneShot(systemSE);
                }
            }

            //次のチェック用にテキスト更新
            beforeText = currentText;

        }).OnComplete(() =>
        {
            isDisplay = true;
        });
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) && isDisplay == true);
        ExitSystemText();
        eventBase.isEventPlay = false;
    }

    private void ExitSystemText()
    {
        textBox.enabled = false;
        systemText.text = string.Empty;
        isDisplay = false;
    }


    public void PrepareSystemSelect(bool heal, bool shop, EventBase eventBase)
    {
        if (heal == true) //回復ポイントだったら
        {
            playerStatus.HealHP();
        }
        if (shop == true) //店だったら
        {

        }
    }
}
