using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback_Player : MonoBehaviour
{
    public float knockbackForce = 0.5f;         // ノックバックの力
    public float knockbackUpwardForce = 0.5f;   // 上方向のノックバックの力
    public float knockbackDuration = 0.4f;      // 無敵時間の長さ
    public AudioClip damageSE;                  // プレイヤーのダメージSE

    public Material mutekiMaterial;             // 無敵用のマテリアル
    public List<GameObject> childrenToChange;   // インスペクタから設定する

    public PlayerBodyController playerBodyController;

    private bool isInvulnerable = false;
    private float invulnerableTimer = 0f;
    private List<Material> defaultMaterials = new List<Material>();
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();


    private Rigidbody2D rb;
    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 子オブジェクトのSpriteRendererコンポーネントを取得し、デフォルトのマテリアルを保存
        foreach (var child in childrenToChange)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteRenderers.Add(sr);
                defaultMaterials.Add(sr.material);
            }
        }
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerableTimer += Time.deltaTime;
            if (invulnerableTimer >= knockbackDuration)
            {
                isInvulnerable = false;
                invulnerableTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            rb.AddForce(new Vector2(5, 5), ForceMode2D.Impulse);
            //rb.AddForce(new Vector2(3, 0));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isInvulnerable)
        {
            // プレイヤーが敵に触れた時の処理
            Vector2 knockback_Hit = (transform.position - other.transform.position).normalized;

            playerBodyController.MoveControl(true);

            if (knockback_Hit.x < 0)
            {
                // ノックバック方向が右
                rb.AddForce(new Vector2(-4, 7), ForceMode2D.Impulse);
            }
            else
            {
                
                // ノックバック方向が左
                rb.AddForce(new Vector2(4, 7), ForceMode2D.Impulse);
            }
            // プレイヤーがダメージを受ける
            PlayerDamage();

            // プレイヤーを無敵状態にする
            isInvulnerable = true;
            invulnerableTimer = 0f;

            // シーケンスの作成
            Sequence sequence = DOTween.Sequence();
            // アニメーションの再生
            for (int i = 0; i < 4; i++)
            {
                // マテリアルをcustomMaterialに切り替え
                sequence.AppendCallback(() => ChangeMaterials(mutekiMaterial));
                sequence.AppendInterval(0.4f / 4f); // 0.4秒の4分の1の間隔で切り替え

                // マテリアルをdefaultMaterialに切り替え
                sequence.AppendCallback(() => ResetMaterials());
                sequence.AppendInterval(0.4f / 4f); // 0.4秒の4分の1の間隔で切り替え
            }
        }
    }

    void PlayerDamage()
    {
        // プレイヤーのダメージSE
        AudioSource.PlayClipAtPoint(damageSE, transform.position);
    }


    void ChangeMaterials(Material newMaterial)
    {
        foreach (var sr in spriteRenderers)
        {
            sr.material = newMaterial;
        }
    }

    void ResetMaterials()
    {
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].material = defaultMaterials[i];
        }
    }
}