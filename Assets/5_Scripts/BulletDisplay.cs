using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletDisplay : MonoBehaviour
{
    public GameObject bulletPrefab; // 通常の弾のPrefab
    public GameObject specialBulletPrefab; // 専用の大きな弾のPrefab
    public int bulletCount; // 現在の弾数

    public float startX = 165f;
    public float startY = -93f;
    public float offsetX = 30f;

    private List<GameObject> bullets = new List<GameObject>();

    public void UpdateBulletDisplay(int amount)
    {
        bulletCount = amount;

        // 既存の弾オブジェクトを削除
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        bullets.Clear();

        // 特別な弾の数を計算
        int specialBulletCount = bulletCount / 20;
        // 通常の弾の数を計算（特別な弾でカバーされない分）
        int regularBulletCount = bulletCount % 20;

        // 特別な弾オブジェクトを生成
        for (int i = 0; i < specialBulletCount; i++)
        {
            GameObject newSpecialBullet = Instantiate(specialBulletPrefab, transform);
            RectTransform specialBulletRect = newSpecialBullet.GetComponent<RectTransform>();
            specialBulletRect.anchoredPosition = new Vector2(startX + (i * offsetX), startY);

            bullets.Add(newSpecialBullet);
        }

        // 通常の弾オブジェクトを生成
        for (int i = 0; i < regularBulletCount; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            RectTransform bulletRect = newBullet.GetComponent<RectTransform>();
            // 特別な弾の分だけオフセットを加算して位置を調整
            bulletRect.anchoredPosition = new Vector2(startX + ((i + specialBulletCount) * offsetX), startY);

            bullets.Add(newBullet);
        }
    }
}