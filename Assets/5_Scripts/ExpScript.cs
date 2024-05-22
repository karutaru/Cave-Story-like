using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpScript : MonoBehaviour
{
    public int expValue;

    private Rigidbody2D rb;
    private Animator animator;

    public AudioClip expBoundSE;

    public AudioClip expGetSE;

    public bool isRemoveExp;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        this.transform.localScale = new Vector3 (this.transform.localScale.x + expValue * 0.01f, this.transform.localScale.y + expValue * 0.01f, 1);

        int randomValue = Random.Range(-80, 81);
        int randomUpValue = Random.Range(1, 181);

        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, Random.Range(0f, 6f));
        rb.AddForce(new Vector3(randomValue, 0, 0));
        rb.AddForce(new Vector3(0, randomUpValue, 0));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        AudioSource.PlayClipAtPoint(expBoundSE, transform.position);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out PlayerStatus playerStatus))
        {
            AudioSource.PlayClipAtPoint(expGetSE, transform.position);

            playerStatus.AddExp(expValue);

            Destroy(this.gameObject);
        }
    }
}
