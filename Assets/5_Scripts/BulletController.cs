using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [LabelText("壁に当たると消える"), HideLabel]
    public bool isWallHit = true;

    private int weaponDamage;
    public int WeaponDamage => weaponDamage; // プロパティ

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

    private void Start()
    {
        if (!isWallHit)
        {
            // 2つ目のCircle Collider 2DをONにする
            CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
            if (colliders.Length > 1)
            {
                colliders[1].enabled = true;
            }
        }

        // パーティクルエフェクトを弾の位置にインスタンス化し、弾の子オブジェクトにする
        foreach (var prefab in particleEffectPrefabs)
        {
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            instance.transform.parent = transform;
            particleEffectInstances.Add(instance);

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

    public void Shoot(Vector2 direction, int maxDamage, int minDamage)
    {
        // 最大値と最小値からダメージを計算し、ダメージセット
        weaponDamage = Random.Range(minDamage, maxDamage + 1);

        // 発射
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") && isWallHit)
        {
            // 壁エフェクトを実体化する
            GameObject wallEffect = Instantiate(GameManager.game.wallEffectPrefab, transform.position, Quaternion.identity);
            // 壁エフェクトを0.2秒後に消す
            Destroy(wallEffect, 0.5f);

            // EffectScriptのインスタンスを取得し、isDestroyメソッドを呼び出す
            isDestroy();

            // プレイヤーの弾を破壊
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Breakable"))
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // 出血エフェクトをリスト内の全てのプレハブから実体化し、親オブジェクトを設定する
            foreach (var bloodEffectPrefab in GameManager.game.bloodEffectPrefabs)
            {
                GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
                bloodEffect.transform.SetParent(other.transform); // 親オブジェクトを設定
                // 出血エフェクトを2秒後に消す
                Destroy(bloodEffect, 2f);
            }

            // EffectScriptのインスタンスを取得し、isDestroyメソッドを呼び出す
            isDestroy();

            // プレイヤーの弾を破壊
            Destroy(this.gameObject, 0.01f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isWallHit || collision.gameObject.CompareTag("Breakable") && !isWallHit)
        {
            //MMFPlayer_Hit.PlayFeedbacks();
            AudioSource.PlayClipAtPoint(GameManager.game.shotHitSE, transform.position);

            // 出血エフェクトをリスト内の全てのプレハブから実体化し、親オブジェクトを設定する
            foreach (var bloodEffectPrefab in GameManager.game.bloodEffectPrefabs)
            {
                GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
                bloodEffect.transform.SetParent(collision.transform); // 親オブジェクトを設定
                // 出血エフェクトを2秒後に消す
                Destroy(bloodEffect, 2f);
            }

            // EffectScriptのインスタンスを取得し、isDestroyメソッドを呼び出す
            isDestroy();

            // プレイヤーの弾を破壊
            Destroy(this.gameObject);
        }
    }

    public void isDestroy()
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