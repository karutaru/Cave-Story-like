using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class E_Button : MonoBehaviour
{
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Image face;
    [SerializeField]
    private Text talkText;
    private bool isDisplay;
    private bool isStay;
    private int zero;
    private int counter;



    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isStay = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isStay = false;
        }
    }


    void Update()
    {
        if (isStay == true && isDisplay == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && counter == 0)
            {
                counter ++;

                textBox.enabled = true;
                face.enabled = true;
                talkText.enabled = true;

                talkText.DOText("(何の為のオブジェクトだろう…?)", 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isDisplay = true;
                counter = 0;
            });
            }
        }

        if (isDisplay == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                textBox.enabled = false;
                face.enabled = false;
                talkText.text = zero.ToString();
                talkText.enabled = false;
                isDisplay = false;
            }
        }
    }
}
