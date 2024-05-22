using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; // DoTween���g�p���邽�߂ɕK�v

public class HeartsHealthDisplay : MonoBehaviour
{
    public int maxHealth = 5; // �v���C���[�̍ő�HP
    public int currentHealth; // ���݂�HP
    public Sprite heartSprite; // �n�[�g�̃X�v���C�g
    public Sprite blackHeartSprite; // �����n�[�g�̃X�v���C�g
    public Sprite whiteHeartSprite; // �����n�[�g�̃X�v���C�g
    public GameObject heartsContainer; // �n�[�g��z�u����e�I�u�W�F�N�g
    public GameObject weaponIcon; // WeaponIcon�I�u�W�F�N�g

    private GameObject[] heartObjects; // �n�[�g��GameObject�z��

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
            int row = i / 20; // 20���ƂɐV�����s
            int column = i % 20; // 20�܂œ����s�ɔz�u
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
            float delay = (startHealth - 1 - i) * 0.1f; // �E���獶�ւ̒x�����v�Z
            heartImage.DOFade(0, 0.2f).SetDelay(delay).SetEase(Ease.InOutQuad); // �x����ݒ肵�ăt�F�[�h�A�E�g
        }
    }

    void AdjustWeaponIconPosition()
    {
        int rows = (maxHealth + 19) / 20; // �K�v�ȍs�����v�Z
        Vector2 newPosition = new Vector2(weaponIcon.transform.position.x, weaponIcon.transform.position.y - rows * 27);
        weaponIcon.transform.position = newPosition;
    }
}
