using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class EffectTest : MonoBehaviour
{
    [Title("プレハブ設定")]
    [Tooltip("インスタンシエイトするプレハブのリスト")]
    public GameObject[] effectPrefabs;

    [Title("インスタンシエイト設定")]
    [Tooltip("インスタンシエイトする回数")]
    public int instantiateCount = 1;

    [Tooltip("インスタンシエイトの間隔（秒）")]
    public float instantiateInterval = 0.5f;

    [Tooltip("インスタンシエイトする方向")]
    public Vector2[] directions;

    [Tooltip("飛ばす強さ")]
    public float forceStrength = 5f;

    [Tooltip("回転角度")]
    public Vector3 rotation = Vector3.zero;

    private void Start()
    {
        Vector3 position = transform.position;
        StartCoroutine(InstantiateEffects(position));
    }

    private IEnumerator InstantiateEffects(Vector3 position)
    {
        if (instantiateCount == 0)
        {
            while (true)
            {
                foreach (var prefab in effectPrefabs)
                {
                    GameObject instance = Instantiate(prefab, position, Quaternion.Euler(rotation));
                    Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                    if (rb != null && directions.Length > 0)
                    {
                        Vector2 direction = directions[Random.Range(0, directions.Length)];
                        rb.AddForce(direction * forceStrength, ForceMode2D.Impulse);
                    }
                }
                yield return new WaitForSeconds(instantiateInterval);
            }
        }
        else
        {
            for (int i = 0; i < instantiateCount; i++)
            {
                foreach (var prefab in effectPrefabs)
                {
                    GameObject instance = Instantiate(prefab, position, Quaternion.Euler(rotation));
                    Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                    if (rb != null && directions.Length > 0)
                    {
                        Vector2 direction = directions[Random.Range(0, directions.Length)];
                        rb.AddForce(direction * forceStrength, ForceMode2D.Impulse);
                    }
                }
                yield return new WaitForSeconds(instantiateInterval);
            }
        }
    }
}
