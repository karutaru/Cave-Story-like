using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public GameObject effect; // エフェクトのゲームオブジェクト

    public float effectInterva = 0.3f; // エフェクトの発生間隔
    public float effectScale = 1.0f; // エフェクトの大きさ
    public int effectFrequency = 0; // エフェクトの表示回数(0なら無限)
    public float effectRandomPos = 0.5f; // エフェクトの発生位置のランダム性
    public float effectRandomScale = 0.2f; // エフェクトの大きさのランダム性

    private int currentEffectCount = 0; // 現在のエフェクト表示回数

    void Start()
    {
        StartCoroutine(SpawnEffects());
    }

    IEnumerator SpawnEffects()
    {
        while (effectFrequency == 0 || currentEffectCount < effectFrequency)
        {
            // エフェクトのインスタンシエイト
            GameObject newEffect = Instantiate(effect, GetRandomPosition(), Quaternion.identity);

            // エフェクトの大きさを設定
            float randomScale = effectScale + Random.Range(-effectRandomScale, effectRandomScale);
            newEffect.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // エフェクトの表示回数をカウント
            currentEffectCount++;

            // 次のエフェクトまで待機
            yield return new WaitForSeconds(effectInterva);
        }
    }

    Vector3 GetRandomPosition()
    {
        // 現在の位置を基準にランダムな位置を生成
        Vector3 randomPos = new Vector3(
            transform.position.x + Random.Range(-effectRandomPos, effectRandomPos),
            transform.position.y + Random.Range(-effectRandomPos, effectRandomPos),
            transform.position.z
        );
        return randomPos;
    }
}
