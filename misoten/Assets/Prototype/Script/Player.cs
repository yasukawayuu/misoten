
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    Rigidbody2D rigid2d;
    Vector2 startPos;

    [SerializeField] float speed = 1f;
    bool _isDrag = false;
    bool _isNoraml = true;
    private LineRenderer _rend;
    private float _scale = 1.0f;
    SpriteRenderer _spriteRenderer;

    [SerializeField]bool _canMove;


    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();
        this._rend = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rend.positionCount = 2;
        
    }

    void Update()
    {
        if(_canMove)
            Move();
    }

    void FixedUpdate()
    {
        this.rigid2d.velocity *= 0.85f;
        transform.localScale = new Vector3(_scale, _scale, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Garbage" && _isNoraml)
        {
            Destroy(collision.gameObject);
            _scale += 0.5f;
            _spriteRenderer.color = Color.red;
            _isNoraml = false;
            StartCoroutine("Clean");
        }
    }

    private void Move()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // マウスを押した地点の座標を記録
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
            _rend.enabled = true;
            _rend.SetPosition(0, new Vector3(worldMousePosition.x, worldMousePosition.y, 0));
            _isDrag = true;
        }

        if (Input.GetMouseButton(0) && _isDrag)
        {
            // 終了点をマウスの現在位置に設定
            _rend.SetPosition(1, new Vector3(worldMousePosition.x, worldMousePosition.y, 0));
        }

        // マウスを離した地点の座標から、発射方向を計算
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 startDirection = -1 * (endPos - startPos).normalized;
            float distance = Vector2.Distance(endPos, startPos);
            this.rigid2d.AddForce(startDirection * distance * speed);
            _rend.enabled = false;
        }
    }

    private IEnumerator Clean()
    {
        yield return new WaitForSeconds(5f);
        _isNoraml = true;
        _spriteRenderer.color = new Color(0.7600026f,1.0f,0.0f);
    }
}