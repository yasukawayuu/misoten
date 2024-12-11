using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Marimo
{
    private Rigidbody2D rigid2d;
    private Vector2 direction;  // AIが移動する方向

    [SerializeField]private float speed = 1.0f;   // 移動速度
    [SerializeField]private float detectionRange = 5f;   // ターゲットを検出する範囲
    private float _scale = 1.0f;   // 大きさ
    private bool _isNoraml = true;   // 通常状態フラグ
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("MarimoAIMove");

    }

    protected override void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        this.rigid2d.velocity *= 0.85f;
        transform.localScale = new Vector3(_scale, _scale, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 通常時ごみに衝突
        if (collision.gameObject.tag == "Garbage" && _isNoraml)
        {
            Destroy(collision.gameObject);
            _scale += 0.5f;
            _spriteRenderer.color = Color.red;
            _isNoraml = false;
            _point += 1;
            StartCoroutine("Clean");
        }
        else if (collision.gameObject.tag == "Garbage" && !_isNoraml)
        {
            // 消滅
            Destroy(this.gameObject);
        }
    }

    // AIの移動ロジック
    protected override void Move()
    {
        // ターゲットが範囲内にあるかをチェック
        Transform target = FindTargetInRange();
        if (target != null)
        {
            // ターゲットの方向に移動
            Vector2 targetDirection = (target.position - transform.position).normalized;
            rigid2d.AddForce(targetDirection * 1000.0f * speed);
        }
        else
        {
            // ランダムな方向に移動
            rigid2d.AddForce(direction * 1000.0f * speed);
        }
    }

    // 新しいランダムな方向を設定する
    private void SetNewDirection()
    {
        float angle = Random.Range(0, 360);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    // 一定範囲内のターゲットを検出する
    private Transform FindTargetInRange()
    {
        // ごみやプレイヤーをタグで見つける
        GameObject[] garbageObjects = GameObject.FindGameObjectsWithTag("Garbage");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Transform closestTarget = null;
        float closestDistance = detectionRange;

        // ごみオブジェクトをチェック
        foreach (GameObject garbage in garbageObjects)
        {
            float distance = Vector2.Distance(transform.position, garbage.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = garbage.transform;
            }
        }

        // プレイヤーもターゲットとしてチェック
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestTarget = player.transform;
            }
        }

        return closestTarget;
    }

    private IEnumerator Clean()
    {
        yield return new WaitForSeconds(5f);
        _isNoraml = true;
        _spriteRenderer.color = new Color(0.0f, 0.6906614f, 1.0f);
    }

    private IEnumerator MarimoAIMove()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            SetNewDirection(); // 一定時間ごとに新しい方向を設定
            Move();
        }
    }
}
