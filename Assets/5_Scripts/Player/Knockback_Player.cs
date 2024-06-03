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
    public List<GameObject> dodgeChange;        // インスペクタから設定する

    public PlayerBodyController playerBodyController;
    public PlayerStatus playerStatus;

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


        // dodgeChangeリストの各GameObjectからSpriteRendererを取得し、リストに追加
        foreach (GameObject obj in dodgeChange)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
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
        PlayerKnockback(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerKnockback(other);
    }

    public void PlayerKnockback(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isInvulnerable)
        {
            // プレイヤーが敵に触れた時の処理
            Vector2 knockback_Hit = (transform.position - other.transform.position).normalized;

            playerBodyController.MoveControl(true);

            if (knockback_Hit.x < 0)
            {
                // ノックバック方向が右
                rb.AddForce(new Vector2(-4f, 6), ForceMode2D.Impulse);
            }
            else
            {

                // ノックバック方向が左
                rb.AddForce(new Vector2(4f, 6), ForceMode2D.Impulse);
            }
            // プレイヤーがダメージを受ける
            PlayerDamage(other);

            PlayerDodge();

            // シーケンスの作成
            Sequence sequence = DOTween.Sequence();
            // アニメーションの再生
            for (int i = 0; i < 5; i++)
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

    public void PlayerDodge()
    {
        // プレイヤーを無敵状態にする
        isInvulnerable = true;
        invulnerableTimer = 0f;
    }

    void PlayerDamage(Collider2D other)
    {
        // プレイヤーのダメージSE
        AudioSource.PlayClipAtPoint(damageSE, transform.position);

        playerStatus.HitEnemy(other);
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