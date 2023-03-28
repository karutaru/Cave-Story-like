using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public PlayerController playerController;
    private bool isUp;
    private bool firstUp;
    private bool isDown;
    private bool firstDown;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Wか↑を押した時
        {
            if (isDown == true && firstDown == true)
            {
                isDown = false;

                // プレイヤーの向いている方向を取得する
                Vector2 directionm = new Vector2 (playerController.playerLookDirection, 0);

                //位置を変更
                Transform myTransformm = this.transform;
                Vector2 posm = new Vector2 (myTransformm.position.x + (0.35f * directionm.x), myTransformm.position.y - -0.3f);
                myTransformm.position = posm;

                //回転を変更
                Vector3 localAnglem = myTransformm.localEulerAngles;
                Vector3 pos2m = new Vector3 (0, 0, localAnglem.z - 90f);
                myTransformm.localEulerAngles = pos2m;
            }

            isUp = true;
            firstUp = true;

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

        if (Input.GetKeyUp(KeyCode.W) && isUp == true) // Wか↑を離した時
        {
            isUp = false;
            Debug.Log("なんでや");

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
            if (isUp == true && firstUp == true)
            {
                isUp = false;

                // プレイヤーの向いている方向を取得する
                Vector2 directionm = new Vector2 (playerController.playerLookDirection, 0);

                //位置を変更
                Transform myTransformm = this.transform;
                Vector2 posm = new Vector2 (myTransformm.position.x + (0.35f * directionm.x), myTransformm.position.y + -0.76f);
                myTransformm.position = posm;

                //回転を変更
                Vector3 localAnglem = myTransformm.localEulerAngles;
                Vector3 pos2m = new Vector3 (0, 0, localAnglem.z + 90f);
                myTransformm.localEulerAngles = pos2m;
            }

            isDown = true;
            firstDown = true;

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

        if (Input.GetKeyUp(KeyCode.S) && isDown == true) // Sか↓を離した時
        {
            isDown = false;

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
