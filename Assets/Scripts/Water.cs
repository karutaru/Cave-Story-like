using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public bool inWater;                            //水に入った
    public WaterController waterController;         //水中呼吸
    public PlayerController playerController;       //移動関連



    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (inWater == false)
            {
                Debug.Log("水に触れている");
                inWater = true;

                waterController.WaterAir(inWater);
                playerController.WaterMove(inWater);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (inWater == true)
            {
                Debug.Log("水から出た");
                inWater = false;

                waterController.WaterAir(inWater);
                playerController.WaterMove(inWater);
            }
        }
    }
}
