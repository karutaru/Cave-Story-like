using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("DestroyObj");
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.3f);

        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
