using UnityEngine;

public class EffectTest : MonoBehaviour
{
    public GameObject effectPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Instantiate(effectPrefab, mousePosition, Quaternion.identity);
        }
    }
}
