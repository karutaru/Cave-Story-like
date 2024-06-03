using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback_Player : MonoBehaviour
{
    public float knockbackForce = 0.5f;         // �m�b�N�o�b�N�̗�
    public float knockbackUpwardForce = 0.5f;   // ������̃m�b�N�o�b�N�̗�
    public float knockbackDuration = 0.4f;      // ���G���Ԃ̒���
    public AudioClip damageSE;                  // �v���C���[�̃_���[�WSE

    public Material mutekiMaterial;             // ���G�p�̃}�e���A��
    public List<GameObject> dodgeChange;        // �C���X�y�N�^����ݒ肷��

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


        // dodgeChange���X�g�̊eGameObject����SpriteRenderer���擾���A���X�g�ɒǉ�
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
            // �v���C���[���G�ɐG�ꂽ���̏���
            Vector2 knockback_Hit = (transform.position - other.transform.position).normalized;

            playerBodyController.MoveControl(true);

            if (knockback_Hit.x < 0)
            {
                // �m�b�N�o�b�N�������E
                rb.AddForce(new Vector2(-4f, 6), ForceMode2D.Impulse);
            }
            else
            {

                // �m�b�N�o�b�N��������
                rb.AddForce(new Vector2(4f, 6), ForceMode2D.Impulse);
            }
            // �v���C���[���_���[�W���󂯂�
            PlayerDamage(other);

            PlayerDodge();

            // �V�[�P���X�̍쐬
            Sequence sequence = DOTween.Sequence();
            // �A�j���[�V�����̍Đ�
            for (int i = 0; i < 5; i++)
            {
                // �}�e���A����customMaterial�ɐ؂�ւ�
                sequence.AppendCallback(() => ChangeMaterials(mutekiMaterial));
                sequence.AppendInterval(0.4f / 4f); // 0.4�b��4����1�̊Ԋu�Ő؂�ւ�

                // �}�e���A����defaultMaterial�ɐ؂�ւ�
                sequence.AppendCallback(() => ResetMaterials());
                sequence.AppendInterval(0.4f / 4f); // 0.4�b��4����1�̊Ԋu�Ő؂�ւ�
            }
        }
    }

    public void PlayerDodge()
    {
        // �v���C���[�𖳓G��Ԃɂ���
        isInvulnerable = true;
        invulnerableTimer = 0f;
    }

    void PlayerDamage(Collider2D other)
    {
        // �v���C���[�̃_���[�WSE
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