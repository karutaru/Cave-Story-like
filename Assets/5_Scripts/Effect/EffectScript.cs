using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

public class EffectScript : MonoBehaviour
{
    [LabelText("パーティクルエフェクトのプレハブ"), HideLabel]
    public GameObject[] particleEffectPrefabs; // パーティクルエフェクトのプレハブの配列
    [LabelText("弾破壊後のパーティクルエフェクトのプレハブ"), HideLabel]
    public GameObject[] particleEffectPrefabs_Last; // 弾破壊後のパーティクルエフェクトのプレハブの配列

    [LabelText("パーティクルエフェクトの発生場所をワールド空間にする"), HideLabel]
    public bool isWorldSpaceEffect = true;  // パーティクルエフェクトの発生場所をワールド空間にする

    [LabelText("大きさの設定方法"), HideLabel]
    public SizeMode sizeMode = SizeMode.Constant; // 大きさの設定方法

    [ShowIf("sizeMode", SizeMode.Constant)]
    [LabelText("倍率"), HideLabel]
    public float sizeMultiplier = 1.0f; // 大きさの倍率

    [ShowIf("sizeMode", SizeMode.RandomBetweenTwoConstants)]
    [LabelText("最小倍率"), HideLabel]
    public float minSize = 0.5f; // 最小倍率
    [ShowIf("sizeMode", SizeMode.RandomBetweenTwoConstants)]
    [LabelText("最大倍率"), HideLabel]
    public float maxSize = 1.5f; // 最大倍率

    [ShowIf("sizeMode", SizeMode.Curve)]
    [LabelText("サイズカーブ"), HideLabel]
    public AnimationCurve sizeCurve = AnimationCurve.Linear(0, 1, 1, 1); // サイズカーブ

    // パーティクルエフェクトのインスタンスのリスト
    private List<GameObject> particleEffectInstances = new List<GameObject>();
    // 弾破壊後のパーティクルエフェクトのインスタンスのリスト
    private List<GameObject> particleEffectInstances_Last = new List<GameObject>();

    void Start()
    {
        // パーティクルエフェクトを弾の位置にインスタンス化し、弾の子オブジェクトにする
        foreach (var prefab in particleEffectPrefabs)
        {
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            instance.transform.parent = transform;
            particleEffectInstances.Add(instance);

            Debug.Log("Particle effect instantiated at position: " + transform.position);

            if (isWorldSpaceEffect)
            {
                // パーティクルシステムのSimulation SpaceをWorldに設定
                var particleSystem = instance.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var mainModule = particleSystem.main;
                    mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
                }
            }
        }
    }

    public void OnDestroy()
    {
        float size = sizeMultiplier;
        if (sizeMode == SizeMode.RandomBetweenTwoConstants)
        {
            size = Random.Range(minSize, maxSize);
        }
        else if (sizeMode == SizeMode.Curve)
        {
            size = sizeCurve.Evaluate(Time.time);
        }

        foreach (var prefab in particleEffectPrefabs_Last)
        {
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            instance.transform.localScale = new Vector3(size, size, size);
            particleEffectInstances_Last.Add(instance);
        }

        // 弾が破壊されたときにパーティクルエフェクトの親子関係を解除し、一定時間後に破壊する
        foreach (var instance in particleEffectInstances)
        {
            if (instance != null)
            {
                var particleSystem = instance.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    // パーティクルシステムを停止し、新しいパーティクルの生成を止める
                    particleSystem.Stop();

                    // パーティクルシステムの親子関係を解除
                    instance.transform.parent = null;

                    // パーティクルの寿命を待ってから破壊
                    Destroy(instance, particleSystem.main.startLifetime.constantMax);
                }
            }
        }
    }

    public enum SizeMode
    {
        Constant,
        RandomBetweenTwoConstants,
        Curve
    }
}