using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController_Overlay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追従させたいターゲット")]
    private GameObject target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始時点のターゲットとの距離（オフセット）を取得
        offset = gameObject.transform.position - target.transform.position;
    }

    /// <summary>
    /// プレイヤーが移動した後に移動するようにするためにLateUpdateにする。
    /// </summary>
    void LateUpdate()
    {
        // 位置をターゲットの位置にオフセットを足した場所にする。
        gameObject.transform.position = target.transform.position + new Vector3(0, 1f, 0);
    }
}
