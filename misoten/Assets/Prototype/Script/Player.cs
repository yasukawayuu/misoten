
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Marimo
{

    private Rigidbody2D rigid2d;
    private Vector2 startPos;

    [SerializeField]private float speed = 1f;

    [SerializeField]private bool _isLocalPlayer = false;
    private bool _isDrag = false;
    private bool _isNoraml = true;

    private LineRenderer _lineRend;
    private Renderer _render;
    private float _scale = 1.0f;
    private SpriteRenderer _spriteRenderer;

    public bool IsLocalPlayer
    {
        get { return _isLocalPlayer; }
        set { _isLocalPlayer = value; }
    }

    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();
        this._lineRend = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lineRend.enabled = false;

        _lineRend.positionCount = 2;
        
        _render = GetComponent<Renderer>();
        _render.sortingOrder = 1;

        _name = "Player";
    }

    protected override void Update()
    {
        if(_isLocalPlayer)
            Move();

        base.Update();
    }

    void FixedUpdate()
    {
        this.rigid2d.velocity *= 0.85f;
        transform.localScale = new Vector3(_scale, _scale, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //汚染物にあったら大きくなり赤くなる
        if (collision.gameObject.tag == "Garbage" && _isNoraml)
        {
            Destroy(collision.gameObject);
            _scale += 0.5f;
            _spriteRenderer.color = Color.red;
            _isNoraml = false;
            _point += 1;
            StartCoroutine("Clean");
        }
        else if(collision.gameObject.tag == "Garbage" && !_isNoraml)
        {
            SceneManager.LoadScene("PrototypeTitle");
        }
    }

    protected override void Move()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // マウスを押した地点の座標を記録
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
            _lineRend.enabled = true;
            _lineRend.SetPosition(0, new Vector3(worldMousePosition.x, worldMousePosition.y, 0));
            _isDrag = true;
        }

        if (Input.GetMouseButton(0) && _isDrag)
        {
            // 終了点をマウスの現在位置に設定
            _lineRend.SetPosition(1, new Vector3(worldMousePosition.x, worldMousePosition.y, 0));
        }

        // マウスを離した地点の座標から、発射方向を計算
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 startDirection = -1 * (endPos - startPos).normalized;
            float distance = Vector2.Distance(endPos, startPos);
            this.rigid2d.AddForce(startDirection * distance * speed);
            _lineRend.enabled = false;
        }
    }
    
    //五秒後に普通状態に戻る
    private IEnumerator Clean()
    {
        yield return new WaitForSeconds(5f);
        _isNoraml = true;
        _spriteRenderer.color = new Color(0.7600026f,1.0f,0.0f);
    }
}