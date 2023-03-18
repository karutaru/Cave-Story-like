using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletDisplay : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount;

    private float startX = 0f;
    private float startY = 0f;
    private float offsetX = 30f;

    private List<GameObject> bullets = new List<GameObject>();
    public BulletCountController bulletCountController;


    public void UpdateBulletDisplay(int amount)
    {
        bulletCount = amount;
        
        // 既存の弾オブジェクトを削除
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        bullets.Clear();

        // 弾数に応じて弾オブジェクトを生成
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            RectTransform bulletRect = newBullet.GetComponent<RectTransform>();
            bulletRect.anchoredPosition = new Vector2(startX + (i * offsetX), startY);

            bullets.Add(newBullet);
        }
    }
}