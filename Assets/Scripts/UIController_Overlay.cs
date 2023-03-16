using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController_Overlay : MonoBehaviour
{
    // [SerializeField]
    // public Transform targetTfm;
 
    // private RectTransform myRectTfm;
    // private Vector3 offset = new Vector3(0, 1.4f, 0);
 
    // void Start()
    // {
    //     myRectTfm = GetComponent<RectTransform>();
    // }
 
    // void Update()
    // {
    //     myRectTfm.position 
    //         = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTfm.position + offset);
    // }

    [SerializeField]
    [Tooltip("追従させたいターゲット")]
    private GameObject target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始時点のカメラとターゲットの距離（オフセット）を取得
        offset = gameObject.transform.position - target.transform.position;
    }

    /// <summary>
    /// プレイヤーが移動した後にカメラが移動するようにするためにLateUpdateにする。
    /// </summary>
    void LateUpdate()
    {
        // カメラの位置をターゲットの位置にオフセットを足した場所にする。
        gameObject.transform.position = target.transform.position + offset;
    }
}
