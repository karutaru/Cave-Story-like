using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Wか↑を押した時
        {
            transform.DOLocalMove(new Vector2(0, 4f), 0.8f) //カメラを4f上にイーズ
            .SetEase(Ease.OutCubic);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Wか↑を離した時
        {
            transform.DOLocalMove(new Vector2(0, 0f), 0.8f) //カメラを4f下にイーズ
            .SetEase(Ease.OutCubic);
        }
    }
}
