using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Button : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }
    }
}
