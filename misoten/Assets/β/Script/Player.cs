
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : Marimo
{
    [CustomLabel("最低移動量")]
    [SerializeField] private float _minSpeed = 0.005f;

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
    private bool _isInvincible = false;
    private float _maxLineLength = 0;   //現在のチャージ
    private float _scale = 1.0f;        //プライヤーの大きさ
    private float _oldScale = 1.0f;
    private float _holdPoint = 0.01f;  //ホールドポイント
    private Vector2 _direction;
    private Vector2 _curretPosition;
    private Vector2 _lastPosition;
    private string _lastHitPlayerName = "";
    private GameObject _lastHitPlayer;

    [SerializeField] private LineRenderer _lineRend;
    [SerializeField] private Renderer _render;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _mapSpriteRender;

    [SerializeField] private PlayerAccessory _playerEyes;
    [SerializeField] private PlayerAccessory _playerBoushi;
    [SerializeField] private PlayerAccessory _playerMegane;
    [SerializeField] private PlayerAccessory _playerHige;

    [SerializeField] private Image _grave;
    [SerializeField] private GameObject[] _disolveObjects;
    [SerializeField] private GameObject _nameText;
    [SerializeField] private GameObject _chargeEffect;
    [SerializeField] private GameObject _burstEffect;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GameObject _deathParticle;

    [SerializeField] private KillLog _killLog;

    [SerializeField] private PlayerGravityController _playerGravityController;

    private float _disolvValue = 1.0f;
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

    public GameObject DeathParticle
    {
        get { return _deathParticle; }
    }

    public float DisolvValue
    {
        get { return _disolvValue; }
    }

    public SpriteRenderer MapRender
    {
        get { return _mapSpriteRender; }
    }

    public Vector2 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public SpriteRenderer EyesSpriteRender
    {
        get { return _playerEyes._accessorySpriteRender; }
    }

    public KillLog KillLOG
    {
        get { return _killLog; }
    }


    public void SetAccessory(Vector3 accessory)
    {
        _playerBoushi._accessorySpriteRender.sprite = _playerBoushi._accessorySprite[(int)accessory.x];
        _playerMegane._accessorySpriteRender.sprite = _playerMegane._accessorySprite[(int)accessory.y];
        _playerHige._accessorySpriteRender.sprite = _playerHige._accessorySprite[(int)accessory.z];
    }

    void Start()
    {
        _playerEyes._accessorySpriteRender.sprite = _playerEyes._accessorySprite[0]; 

        _lineRend.enabled = false;
        _lineRend.positionCount = 2;
        _lineRend.widthMultiplier = 1.0f;
        
        _cameraSize = Camera.main.orthographicSize;

        _nameText.GetComponent<MeshRenderer>().sortingOrder = 2;

        _render.sortingOrder = 2;

        _chargeEffect.SetActive(false);

        _playerGravityController.GravitySetteing(this);

        StartCoroutine("Clean");
    }

    protected override void Update()
    {
        _curretPosition = transform.position;

        float distance = Vector2.Distance(_lastPosition, _curretPosition);
        if (distance < 0.001f)
        {
            if (!(Input.GetMouseButton(0) && _isDrag))
                _playerEyes._accessorySpriteRender.sprite = _playerEyes._accessorySprite[0];

        }
        else
        {
            if (_playerEyes._accessorySpriteRender.sprite  != _playerEyes._accessorySprite[2])
                _playerEyes._accessorySpriteRender.sprite = _playerEyes._accessorySprite[1];
        }

        _lastPosition = _curretPosition;

        if (_isLocalPlayer)
        {
            Move();

            if(_isNormal)
                _grave.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
            else
                _grave.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);

            if (Input.GetKeyDown(KeyCode.Space))
                _point += 1.0f;

            if (Input.GetKeyDown(KeyCode.I))
                _isInvincible = true;

            if (Input.GetKeyDown(KeyCode.O))
                _isInvincible = false;
        }
            
        if (_point > 1.0f)
            _scale = Mathf.Floor(_point) / 5 + 1.0f;
        else
            _scale = 1.0f;

        if (_isLocalPlayer)
            if (_scale != _oldScale)
            {
                _oldScale = _scale;
                ServerManager.Instance.SendSclaeToServer(_scale);
            }

        transform.localScale = new Vector3(_scale, _scale, _scale);

        // _garbageValueを最大値に基づいて、-1から1の範囲に変換
        float gradationValue = Mathf.Lerp(-1.0f, 1.0f, _garbageValue / _maxGarbageValue);

        // シェーダーに値を設定
        _spriteRenderer.material.SetFloat("_Garadation", gradationValue);

        if (_garbageValue >= _maxGarbageValue && !_isInvincible)
            _isNormal = false;


        base.Update();
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
            _playerEyes._accessorySpriteRender.sprite = _playerEyes._accessorySprite[1];
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
                _maxLineLength = 2.0f * cameraSizeRatio;
                _lineRend.material.color = Color.white;
            } 
            else if (distance <= 4.0f * cameraSizeRatio && _point > _pointScale[0])
            {
                _maxLineLength = 4.0f * cameraSizeRatio;
                _lineRend.material.color = Color.yellow;
            }
            else if (distance <= 6.0f * cameraSizeRatio && _point > _pointScale[1])
            {
                _maxLineLength = 6.0f * cameraSizeRatio;
                _lineRend.material.color = Color.green;
            }   
            else if(_point > _pointScale[2])
            {
                _maxLineLength = 8.0f * cameraSizeRatio;
                _lineRend.material.color = Color.blue;
            }

            // スタート位置からマウス位置までの方向と距離を計算
            Vector2 direction = currentMousePos - _startPos;

            // ラインの長さを制限
            Vector2 limitedDirection = Vector2.ClampMagnitude(direction, _maxLineLength);
            Vector2 endPoint = _startPos + limitedDirection;
            
            _lineRend.SetPosition(1, endPoint);

            if(_isLocalPlayer)
                SoundManager.Instance.PlaySE2D("charge", 1.5f);
            // チャージエフェクト表示
            if (!_chargeEffect.activeSelf) _chargeEffect.SetActive(true);
 
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

            // チャージエフェクト非表示
            if (_chargeEffect.activeSelf) _chargeEffect.SetActive(false);
               
            Instantiate(_burstEffect);

            // 力を加える
            if (_maxLineLength > 0.0f)
                ServerManager.Instance.SendInputToServer(startDirection * _holdPoint);

            _lineRend.material.color = Color.white;
            _lineRend.enabled = false;
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
        
    }

    public void EatGarbage()
    {
        if (!_isNormal && _isLocalPlayer)
        {
            StartCoroutine("Disolv");
            Respawn();
        }

        _garbageValue += 1.0f;
        _scale += _point / 5;
        _point += 1.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "Player")
       {
            _lastHitPlayer = collision.gameObject;
            _lastHitPlayerName = collision.gameObject.name;
            // 衝突位置を取得する
            Vector3 hitPos = collision.contacts[0].point;

            Instantiate(_hitEffect, hitPos, Quaternion.identity);

            // 自分の位置と相手の位置
            Vector2 direction = transform.position - collision.gameObject.transform.position;

            // 相手の方向ベクトルが向いている方向
            Vector2 targetDirection = collision.gameObject.GetComponent<Player>().Direction.normalized;

            // 内積を計算
            float dotProduct = Vector2.Dot(targetDirection, direction.normalized);

            // dotProduct の値が -1 と 1 の間に収まるように調整
            float clampedDotProduct = Mathf.Clamp(dotProduct, -1f, 1f);

            // 角度（ラジアン）を計算
            float angleInRadians = Mathf.Acos(clampedDotProduct);

            // ラジアンから度数に変換
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            if(angleInDegrees >= 150f) 
            {
                collision.gameObject.GetComponent<Player>().EyesSpriteRender.sprite = _playerEyes._accessorySprite[2];
                StartCoroutine(collision.gameObject.GetComponent<Player>().TrunEyes());
            }

            SoundManager.Instance.PlaySE3D("hit", hitPos);
       }
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
                {
                    if (_isLocalPlayer)
                        SoundManager.Instance.PlaySE2D("clean");
                    _garbageValue -= 0.1f;
                }

                if (_garbageValue < 0.0f)
                {
                    _garbageValue = 0.0f;
                }
                    
            }  
            else
            {
                yield return new WaitForSeconds(_cleanTime[1]);
                _garbageValue -= 0.1f;

                if (_isLocalPlayer)
                    SoundManager.Instance.PlaySE2D("clean", 3);

                if (_garbageValue <= 0.0f)
                {
                    _isNormal = true;
                    _garbageValue = 0.0f;
                    _maxGarbageValue = Mathf.FloorToInt(_point) * 2;
                }
                
            }
        }
        

    }

    public IEnumerator TrunEyes()
    {
        yield return new WaitForSeconds(0.5f);
        _playerEyes._accessorySpriteRender.sprite = _playerEyes._accessorySprite[0];
    }

    public IEnumerator Disolv()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            _disolvValue -= 0.01f;
            _spriteRenderer.material.SetFloat("_Disolve", _disolvValue);

            if (_disolvValue < 0.0f)
                Destroy(this.gameObject);
        }
    }

    public void SetKillLog()
    {
        if(_lastHitPlayer != null)
            _lastHitPlayer.GetComponent<Player>().KillLOG.SetKillLog(_lastHitPlayerName, this.gameObject.name);

        KillLogList.Instance.SetKillLogList(_lastHitPlayerName, this.gameObject.name);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    /// <returns></returns>
    private void Respawn()
    {
        ServerManager.Instance.CloseWebSocket();
        GameSceneManager gameSceneManager = GameSceneManager.Instance;
        gameSceneManager.IsFade = false;
        gameSceneManager.SceneName = "Title";
    }

    [System.Serializable]
    private class PlayerAccessory
    {
        public SpriteRenderer _accessorySpriteRender;
        public Sprite[] _accessorySprite;
    }
}