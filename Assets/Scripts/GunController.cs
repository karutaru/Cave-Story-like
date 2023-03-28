using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public PlayerController playerController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Wか↑を押した時
        {
            // プレイヤーの向いている方向を取得する
            Vector2 direction = new Vector2 (playerController.playerLookDirection, 0);

            //位置を変更
            Transform myTransform = this.transform;
            Vector2 pos = new Vector2 (myTransform.position.x + (-0.35f * direction.x), myTransform.position.y + 0.76f);
            myTransform.position = pos;

            //回転を変更
            Vector3 localAngle = myTransform.localEulerAngles;
            Vector3 pos2 = new Vector3 (0, 0, localAngle.z + -90f);
            myTransform.localEulerAngles = pos2;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Wか↑を離した時
        {
            // プレイヤーの向いている方向を取得する
            Vector2 direction = new Vector2 (playerController.playerLookDirection, 0);

            //位置を変更
            Transform myTransform = this.transform;
            Vector2 pos = new Vector2 (myTransform.position.x + (0.35f * direction.x), myTransform.position.y + -0.76f);
            myTransform.position = pos;

            //回転を変更
            Vector3 localAngle = myTransform.localEulerAngles;
            Vector3 pos2 = new Vector3 (0, 0, localAngle.z + 90f);
            myTransform.localEulerAngles = pos2;
        }



        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Sか↓を押した時
        {
            // プレイヤーの向いている方向を取得する
            Vector2 direction = new Vector2 (playerController.playerLookDirection, 0);

            //位置を変更
            Transform myTransform = this.transform;
            Vector2 pos = new Vector2 (myTransform.position.x + (-0.35f * direction.x), myTransform.position.y - 0.3f);
            myTransform.position = pos;

            //回転を変更
            Vector3 localAngle = myTransform.localEulerAngles;
            Vector3 pos2 = new Vector3 (0, 0, localAngle.z + 90f);
            myTransform.localEulerAngles = pos2;
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Sか↓を離した時
        {
            // プレイヤーの向いている方向を取得する
            Vector2 direction = new Vector2 (playerController.playerLookDirection, 0);

            //位置を変更
            Transform myTransform = this.transform;
            Vector2 pos = new Vector2 (myTransform.position.x + (0.35f * direction.x), myTransform.position.y - -0.3f);
            myTransform.position = pos;

            //回転を変更
            Vector3 localAngle = myTransform.localEulerAngles;
            Vector3 pos2 = new Vector3 (0, 0, localAngle.z - 90f);
            myTransform.localEulerAngles = pos2;
        }
    }
}
