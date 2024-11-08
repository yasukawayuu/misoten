
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Marimo
{
    [SerializeField] private float _holdPoint = 900.0f;
    [SerializeField] private float[] _pointScale = new float[3];
    [SerializeField] private int[] _raito = new int[3]; 
   
    private Vector2 _startPos;
    [SerializeField] private bool _isLocalPlayer = false;
    private bool _isDrag = false;
    private bool _isNoraml = true;
    private float _maxLineLength = 0;
    private float _scale = 1.0f;

    [SerializeField] private Rigidbody2D _rigid2d;
    [SerializeField] private LineRenderer _lineRend;
    [SerializeField] private Renderer _render;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _childSpriteRender;


    public bool IsLocalPlayer
    {
        get { return _isLocalPlayer; }
        set { _isLocalPlayer = value; }
    }

    void Start()
    {

        _lineRend.enabled = false;

        _lineRend.positionCount = 2;

        _render.sortingOrder = 1;

        _name = "Player";
    }

    protected override void Update()
    {
        if (_isLocalPlayer)
            Move();

        base.Update();
    }

    void FixedUpdate()
    {
        _rigid2d.velocity *= 0.85f;
        if( _point > 1.0f )
            _scale = Mathf.Floor(_point) / 2 + 0.5f;
        else
            _scale = 1.0f;
        transform.localScale = new Vector3(_scale, _scale, 0.0f);
        float scaledValue = (_garbageValue / _maxGarbageValue) * 2.0f + 1.0f;
        _spriteRenderer.material.SetFloat("_BeforeColorAmount", scaledValue);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(_garbageValue >= _maxGarbageValue)
        //{
        //    SceneManager.LoadScene("PrototypeTitle");
        //}
           
        //汚染物にあったら大きくなり赤くなる
        if (collision.gameObject.tag == "Garbage")
        {
            Destroy(collision.gameObject);
            _garbageValue += 1;
            _scale += _point / 2;
            _point += 1.0f;
            StartCoroutine("Clean");
        }

    }

    //プレイヤーの移動
    protected override void Move()
    {
        // マウスを押した地点の座標を記録
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRend.enabled = true;
            _lineRend.SetPosition(0, _startPos);
            _isDrag = true;
            _maxLineLength = 0.0f;
        }

        // マウスを押している間
        if (Input.GetMouseButton(0) && _isDrag)
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ワールド座標での距離計算
            float distance = Vector2.Distance(currentMousePos, _startPos);

            // ラインの色と長さ制限を設定
            if (distance > 1.0f && distance <= 2.0f)
            {
                _childSpriteRender.color = Color.gray;
                _maxLineLength = 2.0f;
            } 
            else if (distance <= 4.0f && _point > _pointScale[0])
            {
                _childSpriteRender.color = Color.yellow;
                _maxLineLength = 4.0f;
            }
            else if (distance <= 6.0f && _point > _pointScale[1])
            {
                _childSpriteRender.color = Color.green;
                _maxLineLength = 6.0f;
            }   
            else if(_point > _pointScale[2])
            {
                _childSpriteRender.color = Color.blue;
                _maxLineLength = 8.0f;
            }

            // スタート位置からマウス位置までの方向と距離を計算
            Vector2 direction = currentMousePos - _startPos;

            // ラインの長さを制限
            Vector2 limitedDirection = Vector2.ClampMagnitude(direction, _maxLineLength);
            Vector2 endPoint = _startPos + limitedDirection;

            _lineRend.SetPosition(1, endPoint);
        }

        // マウスを離したとき
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 startDirection = -1 * (endPos - _startPos).normalized;

            // ポイント消費
            if (_maxLineLength <= 2.0f)
                _holdPoint = 900.0f;
            else if (_maxLineLength <= 4.0f && _point > _pointScale[0])
                HoldPoint(_raito[0]);
            else if (_maxLineLength <= 6.0f && _point > _pointScale[1])
                HoldPoint(_raito[1]); 
            else if(_point > _pointScale[2])
                HoldPoint(_raito[2]);

            // 力を加える
            if(_maxLineLength > 0.0f)
                _rigid2d.AddForce(startDirection * _holdPoint);

            _lineRend.enabled = false;
            _childSpriteRender.color = Color.white;
        }

    }

    //ホールポイント計算
    private void HoldPoint(int ratio)
    {
        _holdPoint = (Mathf.Floor(_point) / ratio) * 10 + 900.0f;
        _point -= (Mathf.Floor(_point) / ratio);
        _point = Mathf.Floor(_point);
    }

   private void Shader()
    {

    }

    //五秒後に普通状態に戻る
    private IEnumerator Clean()
    {
        yield return new WaitForSeconds(5f);
    }
}