using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private float destroyTime;
    private float timer;

    void Start()
    {
        destroyTime = 0.32f;
        //StartCoroutine("DestroyObj");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destroyTime)
        {
            Destroy(gameObject);
        }
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
