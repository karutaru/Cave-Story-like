using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepCounter : MonoBehaviour
{
    [SerializeField]
    private Text stepCount;
    [SerializeField]
    Text text;
    private int count;


    void Start()
    {
        text.enabled = false;
    }

    public void AddStepCount(int amount)
    {
        count += amount;
        stepCount.text = count.ToString();
    }

    public void CallStepCount(bool judge)
    {
        if (judge == true)
        {
            text.enabled = true;
        }
        else {
            text.enabled = false;
        }
    }
}
