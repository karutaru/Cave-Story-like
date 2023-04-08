using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoadScript : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
