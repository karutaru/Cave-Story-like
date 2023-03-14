using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [Header("ワープ先は+1なら右、-1なら左")]
    public int warpOffset;
    private Vector2 warpPoint;              //ワープ先の位置情報
    public GameObject player;               //プレイヤー情報
    public GameObject warpNextDoor;         //ワープ先のオブジェクト
    public PlayerController playerController;


    void Start()
    {
        GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //ワープ先の情報を代入
            Vector2 warpPoint = new Vector2 (warpNextDoor.transform.position.x + warpOffset, warpNextDoor.transform.position.y);
            //プレイヤーの位置を移動
            player.transform.position = warpPoint;
        }
    }
}
