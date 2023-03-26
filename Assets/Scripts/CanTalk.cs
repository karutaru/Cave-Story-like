using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanTalk : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer canTalk;


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("E_Button"))
        {
            canTalk.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("E_Button"))
        {
            canTalk.enabled = false;
        }
    }
}
