using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public GameObject particleEffectPrefab; // パーティクルエフェクトのプレハブ
    public bool isWorldSpaceEffect = true;  // パーティクルエフェクトの発生場所をワールド空間にする

    // パーティクルエフェクトのインスタンス
    private GameObject particleEffectInstance;

    void Start()
    {
        // パーティクルエフェクトを弾の位置にインスタンス化し、弾の子オブジェクトにする
        particleEffectInstance = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        particleEffectInstance.transform.parent = transform;

        if (isWorldSpaceEffect)
        {
            // パーティクルシステムのSimulation SpaceをWorldに設定
            var particleSystem = particleEffectInstance.GetComponent<ParticleSystem>();
            var mainModule = particleSystem.main;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    public void OnDestroy()
    {
        // 弾が破壊されたときにパーティクルエフェクトの親子関係を解除し、一定時間後に破壊する
        if (particleEffectInstance != null)
        {
            var particleSystem = particleEffectInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                // パーティクルシステムを停止し、新しいパーティクルの生成を止める
                particleSystem.Stop();

                // パーティクルシステムの親子関係を解除
                particleEffectInstance.transform.parent = null;

                // パーティクルの寿命を待ってから破壊
                Destroy(particleEffectInstance, particleSystem.main.startLifetime.constantMax);
            }
        }
    }
}