using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class EffectTest : MonoBehaviour
{
    [Title("�v���n�u�ݒ�")]
    [Tooltip("�C���X�^���V�G�C�g����v���n�u�̃��X�g")]
    public GameObject[] effectPrefabs;

    [Title("�C���X�^���V�G�C�g�ݒ�")]
    [Tooltip("�C���X�^���V�G�C�g�����")]
    public int instantiateCount = 1;

    [Tooltip("�C���X�^���V�G�C�g�̊Ԋu�i�b�j")]
    public float instantiateInterval = 0.5f;

    [Tooltip("�C���X�^���V�G�C�g�������")]
    public Vector2[] directions;

    [Tooltip("��΂�����")]
    public float forceStrength = 5f;

    [Tooltip("��]�p�x")]
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
