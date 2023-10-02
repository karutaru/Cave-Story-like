using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyRotation : MonoBehaviour
{
    public PlayerBodyController controller;
    public PlayerHead playerHead;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && Time.timeScale == 1)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f); //左向き

            playerHead.LookL(true);
        }

        if (Input.GetKeyDown(KeyCode.D) && Time.timeScale == 1)
        {
            this.transform.rotation = Quaternion.Euler(0f, 180f, 0f); //右向き

            playerHead.LookL(false);
        }
    }
}
