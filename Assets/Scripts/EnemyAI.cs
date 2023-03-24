using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float jumpSpeed = 200f;
    public float nextWayPointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    //public float jumpModifier = 0.3f;
    //public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    public bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    [SerializeField, Header("Linecast用 地面判定レイヤー")]
    private LayerMask groundLayer;
    private float jumpTimer;
    private bool jumping;


    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb =GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        //ジャンプ判定
        isGrounded = Physics2D.Linecast(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.6f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * -0.2f, transform.position - transform.up * 0.5f, Color.red, 0.5f);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //ジャンプ
        if (jumpEnabled && isGrounded && jumping == false)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                jumping = true;
                //rb.AddForce(Vector2.up * jumpSpeed * jumpModifier);
                //rb.AddForce(new Vector2 (0, jumpSpeed));
                //rb.AddForce(transform.up * jumpSpeed);
                //現在の座標からY+1.1の座標へ1秒で移動する
                // this.transform.DOMove(endValue: new Vector3(this.transform.position.x + 0.25f * -transform.localScale.x, this.transform.position.y + 1.1f, 0), duration: 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                // {
                //     this.transform.DOMove(endValue: new Vector3(this.transform.position.x + 0.25f * -transform.localScale.x, this.transform.position.y - 1.1f, 0), duration: 0.5f).SetEase(Ease.InQuart);
                // });

                //X軸ジャンプ
                this.transform.DOMoveX(this.transform.position.x + 0.5f * -transform.localScale.x, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    this.transform.DOMoveX(this.transform.position.x + 0.5f * -transform.localScale.x, 0.5f).SetEase(Ease.Linear);
                });

                //Y軸ジャンプ
                this.transform.DOMoveY(this.transform.position.y + 1.1f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    this.transform.DOMoveY(this.transform.position.y - 1.1f, 0.5f).SetEase(Ease.InQuad);
                });

                
            }
        }

        //移動
        //rb.AddForce(force);
        this.gameObject.transform.localPosition = new Vector2 (this.gameObject.transform.localPosition.x + 0.04f * -transform.localScale.x, this.gameObject.transform.localPosition.y);

        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }

        //見ている方向の処理
        if (directionLookEnabled)
        {
            if (direction.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p) {
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
    }

    void Update()
    {
        if (jumping == true)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= 0.3f)
            {
                jumpTimer = 0;
                jumping = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        this.transform.DOKill();
    }
}
