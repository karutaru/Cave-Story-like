using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public int itemID; // アイテムのID

    private bool getItem = false;
    public float waitFlashTime = 0.13f; // 点滅間隔

    private Renderer itemRenderer;
    public AudioClip itemGetSE;


    void Start()
    {
        itemRenderer = gameObject.GetComponent<Renderer>();
        StartCoroutine("Flashing");
    }

    IEnumerator Flashing()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(waitFlashTime * 2);
            itemRenderer.enabled = false;
            yield return new WaitForSeconds(waitFlashTime);
            itemRenderer.enabled = true;
        }
        getItem = true;
        gameObject.layer = 10;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (getItem && col.TryGetComponent(out ItemController itemController))
        {
            //AudioSource.PlayClipAtPoint(itemGetSE, transform.position);
            itemController.AddItem(itemID);

            Destroy(gameObject);
        }
    }
}
