using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private Text damageText;
    [SerializeField] private float disappearTime = 1f;
    [SerializeField] private Vector2 randomOffset = new Vector2(1, 1);

    private float disappearTimer;

    private void Start()
    {
        disappearTimer = disappearTime;
    }

    private void Update()
    {
        disappearTimer -= Time.deltaTime;

        if (disappearTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
        damageText.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.25f);
        transform.localPosition += new Vector3(Random.Range(-randomOffset.x, randomOffset.x), Random.Range(-randomOffset.y, randomOffset.y), 0);
        disappearTimer = disappearTime;
    }
}