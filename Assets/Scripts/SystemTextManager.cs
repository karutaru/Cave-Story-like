using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SystemTextManager : MonoBehaviour
{
    public static SystemTextManager instanceSystemText;
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Text systemText;
    [SerializeField]
    private float messageSpeed = 0.05f;
    private bool isDisplay;



    void Awake()
    {
        if (instanceSystemText == null)
        {
            instanceSystemText = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PrepareSystemMessage(string systemMessage, EventBase eventBase)
    {
        StartCoroutine(SyestemMessage(systemMessage, eventBase));
    }

    private IEnumerator SyestemMessage(string systemMessage, EventBase eventBase)
    {
                textBox.enabled = true;
                systemText.enabled = true;

                systemText.DOText(systemMessage, systemMessage.Length * messageSpeed).SetEase(Ease.Linear).OnComplete(() =>
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
}
