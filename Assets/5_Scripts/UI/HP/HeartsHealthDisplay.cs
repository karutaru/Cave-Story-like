using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; // DoTweenを使用するために必要

public class HeartsHealthDisplay : MonoBehaviour
{
    public int maxHealth = 5; // プレイヤーの最大HP
    public int currentHealth; // 現在のHP
    public Sprite heartSprite; // ハートのスプライト
    public Sprite blackHeartSprite; // 黒いハートのスプライト
    public Sprite whiteHeartSprite; // 白いハートのスプライト
    public GameObject heartsContainer; // ハートを配置する親オブジェクト
    public GameObject weaponIcon; // WeaponIconオブジェクト

    private GameObject[] heartObjects; // ハートのGameObject配列

    void Start()
    {
        currentHealth = maxHealth;
        heartObjects = new GameObject[maxHealth];
        CreateBlackHearts();
        CreateHearts();
        AdjustWeaponIconPosition();
    }

    void CreateHearts()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = new GameObject("Heart" + i);
            heart.transform.SetParent(heartsContainer.transform, false);
            heartObjects[i] = heart;

            Image heartImage = heart.AddComponent<Image>();
            heartImage.sprite = heartSprite;

            RectTransform rectTransform = heart.GetComponent<RectTransform>();
            int row = i / 20; // 20個ごとに新しい行
            int column = i % 20; // 20個まで同じ行に配置
            rectTransform.anchoredPosition = new Vector2(column * 50, -row * 50);
            rectTransform.sizeDelta = new Vector2(40, 40);
        }
    }

    void CreateBlackHearts()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject blackHeart = new GameObject("BlackHeart" + i);
            blackHeart.transform.SetParent(heartsContainer.transform, false);

            Image blackHeartImage = blackHeart.AddComponent<Image>();
            blackHeartImage.sprite = blackHeartSprite;

            RectTransform rectTransform = blackHeart.GetComponent<RectTransform>();
            int row = i / 20;
            int column = i % 20;
            rectTransform.anchoredPosition = new Vector2(column * 50, -row * 50);
            rectTransform.sizeDelta = new Vector2(40, 40);
        }
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(DisplayWhiteHearts(damage));
    }

    IEnumerator DisplayWhiteHearts(int damage)
    {
        int startHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        for (int i = startHealth - 1; i >= currentHealth; i--)
        {
            if (i < 0) break;
            heartObjects[i].GetComponent<Image>().sprite = whiteHeartSprite;
        }

        yield return new WaitForSeconds(1);

        for (int i = startHealth - 1; i >= currentHealth; i--)
        {
            if (i < 0) break;
            Image heartImage = heartObjects[i].GetComponent<Image>();
            float delay = (startHealth - 1 - i) * 0.1f; // 右から左への遅延を計算
            heartImage.DOFade(0, 0.2f).SetDelay(delay).SetEase(Ease.InOutQuad); // 遅延を設定してフェードアウト
        }
    }

    void AdjustWeaponIconPosition()
    {
        int rows = (maxHealth + 19) / 20; // 必要な行数を計算
        Vector2 newPosition = new Vector2(weaponIcon.transform.position.x, weaponIcon.transform.position.y - rows * 27);
        weaponIcon.transform.position = newPosition;
    }
}
