using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    private bool inWatercatch;
    public int air;


    public void WaterAir(bool inWater)
    {
        inWatercatch = inWater;
    }

    void Update()
    {
        if (inWatercatch == true)
        {
            air -= 1;//くうきを減らす
        }
    }
}
