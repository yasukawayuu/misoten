using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMarimo : MonoBehaviour
{
    Rigidbody2D rigid2d;
    Vector2 direction;  // AIが移動する方向
    private float moveTimer = 0.0f; // 移動時間を計測するタイマー
    [SerializeField] float changeDirectionTime = 2.0f; // 一定時間ごとに方向を変える
    [SerializeField] float speed = 1.0f;   // 移動速度
    [SerializeField] float detectionRange = 5f;   // ターゲットを検出する範囲
    private float _scale = 1.0f;   // 大きさ
    bool _isNoraml = true;   // 通常状態フラグ
    SpriteRenderer _spriteRenderer;
    [SerializeField] bool _canMove = true;   // 移動可能フラグ

    int _point = 0;   // 得点
    [SerializeField] string _name = "AI";   // 名前

    public int Point
    {
        get { return _point; }
    }

    public string Name
    {
        get { return _name; }
    }

    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection(); // 最初の移動方向を設定
    }

    void Update()
    {
        if (_canMove)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= changeDirectionTime)
            {
                SetNewDirection(); // 一定時間ごとに新しい方向を設定
                moveTimer = 0f;
            }

            Move(); // 移動
        }

        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, -100.0f, 100.0f);
        currentPos.y = Mathf.Clamp(currentPos.y, -100.0f, 100.0f);
        transform.position = currentPos;
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
        // 浄化時ごみに衝突
        else if (collision.gameObject.tag == "Garbage" && !_isNoraml)
        {
            // 消滅
            Destroy(this.gameObject);
        }
    }

    // AIの移動ロジック
    private void Move()
    {
        // ターゲットが範囲内にあるかをチェック
        Transform target = FindTargetInRange();
        if (target != null)
        {
            // ターゲットの方向に移動
            Vector2 targetDirection = (target.position - transform.position).normalized;
            rigid2d.AddForce(targetDirection * speed);
        }
        else
        {
            // ランダムな方向に移動
            rigid2d.AddForce(direction * speed);
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
}
