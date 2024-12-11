
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Marimo
{
    [CustomLabel("最低移動量")]
    [SerializeField] private float _minSpeed = 0.01f;

    [CustomLabel("ローカルプレイヤー")]
    [SerializeField] private bool _isLocalPlayer = false;

    [CustomLabel("チャージ上限")]
    [SerializeField] private float[] _pointScale = new float[3];

    [CustomLabel("チャージ割合")]
    [SerializeField] private int[] _raito = new int[3];

    [CustomLabel("ポイント消費割合")]
    [SerializeField] private int[] _pointRaito = new int[3];

    [CustomLabel("浄化時間")]
    [SerializeField] private float[] _cleanTime = new float[2];

    private Vector2 _startPos;          //マウスの初期位置
    private bool _isDrag = false;       //マウス押してるかどうか
    private bool _isNormal = true;      //上限に達してるか
    private float _maxLineLength = 0;   //現在のチャージ
    private float _scale = 1.0f;        //プライヤーの大きさ
    private float _holdPoint = 0.01f;　 //ホールドポイント

    [SerializeField] private LineRenderer _lineRend;
    [SerializeField] private Renderer _render;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _childSpriteRender;
    [SerializeField] private SpriteRenderer _eyesSpriteRender;
    [SerializeField] private Sprite[] _eyesSprite = new Sprite[2];
    [SerializeField] private GameObject _nameText;
    [SerializeField] private PlayerGravityController _playerGravityController;

    private float _cameraSize = 0.0f;
    private float _lineWidth = 1.0f;

    public bool IsLocalPlayer
    {
        get { return _isLocalPlayer; }
        set { _isLocalPlayer = value; }
    }

    public GameObject NameText
    {
        get { return _nameText; }
    }

    void Start()
    {
        _eyesSpriteRender.sprite = _eyesSprite[0]; 
        _lineRend.enabled = false;
        _lineRend.positionCount = 2;
        _lineRend.widthMultiplier = 1.0f;
        
        _cameraSize = Camera.main.orthographicSize;

        _nameText.GetComponent<MeshRenderer>().sortingOrder = 2;

        _render.sortingOrder = 2;

        _playerGravityController.GravitySetteing(this);

        StartCoroutine("Clean");
    }

    protected override void Update()
    {
        if (_isLocalPlayer)
            Move();

        base.Update();
    }

    void FixedUpdate()
    {
        if( _point > 1.0f )
            _scale = Mathf.Floor(_point) / 5 + 1.0f;
        else
            _scale = 1.0f;
        transform.localScale = new Vector3(_scale, _scale, 0.0f);

        // _garbageValueを最大値に基づいて、-1から1の範囲に変換
        float gradationValue = Mathf.Lerp(-1.0f, 1.0f, _garbageValue / _maxGarbageValue);

        // シェーダーに値を設定
        _spriteRenderer.material.SetFloat("_Garadation", gradationValue);

        if (_garbageValue >= _maxGarbageValue)
            _isNormal = false;
    }

    /// <summary>
    /// プレイヤーの移動
    /// </summary>    
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
            _eyesSpriteRender.sprite = _eyesSprite[1];
        }

        // マウスを押している間
        if (Input.GetMouseButton(0) && _isDrag)
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // カメラサイズに基づいて距離をスケーリング
            float cameraSizeRatio = Camera.main.orthographicSize / _cameraSize;
            float distance = Vector2.Distance(currentMousePos, _startPos) * cameraSizeRatio;

            // ラインの色と長さ制限を設定
            if (distance <= 2.0f * cameraSizeRatio)
            {
                _childSpriteRender.color = Color.gray;
                _maxLineLength = 2.0f * cameraSizeRatio;
            } 
            else if (distance <= 4.0f * cameraSizeRatio && _point > _pointScale[0])
            {
                _childSpriteRender.color = Color.yellow;
                _maxLineLength = 4.0f * cameraSizeRatio;
            }
            else if (distance <= 6.0f * cameraSizeRatio && _point > _pointScale[1])
            {
                _childSpriteRender.color = Color.green;
                _maxLineLength = 6.0f * cameraSizeRatio;
            }   
            else if(_point > _pointScale[2])
            {
                _childSpriteRender.color = Color.blue;
                _maxLineLength = 8.0f * cameraSizeRatio;
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

            float cameraSizeRatio = Camera.main.orthographicSize / _cameraSize;

            // ポイント消費
            if (_maxLineLength <= 2.0f * cameraSizeRatio)
                _holdPoint = _minSpeed;
            else if (_maxLineLength <= 4.0f * cameraSizeRatio && _point > _pointScale[0])
                HoldPoint(_raito[0], _pointRaito[0]);
            else if (_maxLineLength <= 6.0f * cameraSizeRatio && _point > _pointScale[1])
                HoldPoint(_raito[1], _pointRaito[1]); 
            else if(_point > _pointScale[2])
                HoldPoint(_raito[2], _pointRaito[2]);

            
            // 力を加える
            if (_maxLineLength > 0.0f)
                ServerManager.Instance.SendInputToServer(startDirection * _holdPoint);

            _lineRend.enabled = false;
            _eyesSpriteRender.sprite = _eyesSprite[0];
            _childSpriteRender.color = Color.white;
        }

        _lineRend.widthMultiplier = _lineWidth * (Camera.main.orthographicSize / _cameraSize);

    }

    /// <summary>
    /// ホールドポイント計算
    /// </summary>
    /// <param name="ratio"></param>
    private void HoldPoint(int ratio,int pointRaito)
    {
        _holdPoint = (Mathf.Floor(_point) / ratio) + _minSpeed;
        _point -= (Mathf.Floor(_point) / pointRaito);
        _point = Mathf.Floor(_point);
        ServerManager.Instance.SendSclaeToServer(this.gameObject.transform.localScale.x);
    }

    public void EatGarbage()
    {
        if(!_isNormal && _isLocalPlayer)
        {
            ServerManager.Instance.CloseWebSocket();
            SceneManager.LoadScene("Title");
        }

        _garbageValue += 1;
        _scale += _point / 5;
        _point += 1.0f;
        ServerManager.Instance.SendSclaeToServer(this.gameObject.transform.localScale.x);
    }

    /// <summary>
    /// 浄化
    /// </summary>
    /// <returns></returns>
    private IEnumerator Clean()
    {
        while(true)
        {
            if (_isNormal)
            {
                yield return new WaitForSeconds(_cleanTime[0]);
                if (_garbageValue > 0.0f)
                    _garbageValue -= 0.1f;

                if(_garbageValue < 0.0f)
                    _garbageValue = 0.0f;
            }  
            else
            {
                yield return new WaitForSeconds(_cleanTime[1]);
                _garbageValue -= 0.1f;

                if (_garbageValue <= 0.0f)
                {
                    _isNormal = true;
                    _garbageValue = 0.0f;
                    _maxGarbageValue = Mathf.FloorToInt(_point) * 2;
                }     
            }
        }
        

    }
}