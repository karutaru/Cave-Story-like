using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public int itemID; // �A�C�e����ID

    private bool getItem = false;
    public float waitFlashTime = 0.13f; // �_�ŊԊu

    private Renderer itemRenderer;
    public AudioClip itemGetSE;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemRenderer = gameObject.GetComponent<Renderer>();
        StartCoroutine("Flashing");

        int randomValue = Random.Range(-80, 81);
        int randomUpValue = Random.Range(1, 181);

        rb.AddForce(new Vector3(randomValue, 0, 0));
        rb.AddForce(new Vector3(0, randomUpValue, 0));
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
            // 1�̃A�C�e����ǉ�
            itemController.AddItem(itemID, 1);

            Destroy(gameObject);
        }
    }
}
