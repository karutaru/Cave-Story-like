using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager instance;
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Image face;
    [SerializeField]
    private Text talkText;
    private bool isDisplay;

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

    public void PrepareDisplayMessage(string message, EventBase eventBase)
    {
        StartCoroutine(DisplayMessage(message, eventBase));
    }

    private IEnumerator DisplayMessage(string message, EventBase eventBase)
    {
                textBox.enabled = true;
                face.enabled = true;
                talkText.enabled = true;

                talkText.DOText(message, 1f).SetEase(Ease.Linear).OnComplete(() =>
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
