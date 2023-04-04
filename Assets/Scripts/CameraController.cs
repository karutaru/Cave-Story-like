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
            transform.DOLocalMove(new Vector2(0, 3f), 0.8f) //カメラを3f上にイーズ
            .SetEase(Ease.OutCubic);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Wか↑を離した時
        {
            transform.DOLocalMove(new Vector2(0, 0f), 0.8f) //カメラを3f下にイーズ
            .SetEase(Ease.OutCubic);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) //Sか↓を押した時
        {
            transform.DOLocalMove(new Vector2(0, -3f), 0.8f) //カメラを3f下にイーズ
            .SetEase(Ease.OutCubic);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) //Sか↓を離した時
        {
            transform.DOLocalMove(new Vector2(0, 0f), 0.8f) //カメラを3f上にイーズ
            .SetEase(Ease.OutCubic);
        }
    }
}
