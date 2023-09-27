using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterController : MonoBehaviour
{
    public float breathCounter = 0.15f;
    public int air = 100;
    [SerializeField]
    private Text breath;
    [SerializeField]
    private Text breathLabel;
    private float breathTimer;
    private float textTimer;
    private bool inWatercatch;


    public void WaterAir(bool inWater)
    {
        inWatercatch = inWater;
    }

    void Update()
    {
        if (inWatercatch == true)
        {
            breath.enabled = true;
            breathLabel.enabled = true;

            breathTimer += Time.deltaTime;
            if (breathTimer >= breathCounter)
            {
                if (air <= 0)
                {
                    
                } else {
                    air -= 1;//くうきを減らす
                breath.text = air.ToString();
                breathTimer = 0;
                }
            }

        } else {
            breathLabel.enabled = false;
            breath.enabled = false;
            air = 100;
            breath.text = air.ToString();
            breathTimer = 0;
        }
    }
}
