using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject effectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shell"))
        {
            Debug.Log("弾が当たった");
            if (other != null)
            {
                //弾
                Destroy(other.gameObject);
            }

            // エフェクトを実体化する
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            // エフェクトを0.8秒後に消す
            Destroy(effect, 0.2f);
        }
    }
}
