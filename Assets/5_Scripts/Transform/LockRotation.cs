using UnityEngine;

public class LockRotation : MonoBehaviour
{
    // 初期のローカル回転を保持するための変数
    private Quaternion initialLocalRotation;

    void Start()
    {
        // 初期のローカル回転を保存
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        // 親オブジェクトの回転を逆回転させて、初期のローカル回転を維持
        transform.localRotation = initialLocalRotation * Quaternion.Inverse(transform.parent.rotation);
    }
}