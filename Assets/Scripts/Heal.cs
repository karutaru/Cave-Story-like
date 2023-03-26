using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Heal : MonoBehaviour
{
    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Text systemText;
    private bool isDisplay;
    private bool isStay;
    private int zero;
    private int counter;
    public AudioClip healSE;
    public PlayerStatus playerStatus;



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
                AudioSource.PlayClipAtPoint(healSE, transform.position);

                textBox.enabled = true;
                systemText.enabled = true;

                playerStatus.HealHP();

                systemText.DOText("HPが回復した。", 0.5f).OnComplete(() =>
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
                systemText.text = zero.ToString();
                systemText.enabled = false;
                isDisplay = false;
            }
        }
    }
}
