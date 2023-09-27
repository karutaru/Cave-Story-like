using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandsScript : MonoBehaviour
{
    public bool isGrounded;
    private Animator anim;

    [SerializeField, Header("Linecast用 地面判定レイヤー")]
    private LayerMask groundLayer;



    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //接地判定用のラインキャスト
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.4f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.4f, transform.position - transform.up * 0.6f, Color.red, 0.3f);

        if (isGrounded && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) // 左右キーを押している間
        {
            anim.Play("Hands_Normal_Anim");
        }
        else
        {
            anim.Play("Hands_Normal");
        }
    }

}
