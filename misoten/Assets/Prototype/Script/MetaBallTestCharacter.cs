using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBallTestCharacter : MonoBehaviour
{
    //座標用の変数
    [SerializeField] private Vector3 mousePos, worldPos;
    [SerializeField] private float _garbageValue = 0.0f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private bool _notMove = false;

    void Start()
    {
        if(_spriteRenderer == null)
        {
            Debug.LogWarning("_spriteRenderer is null");
        }
    }

    void FixedUpdate()
    {
        if(_garbageValue <0.0f)
            _garbageValue += 0.001f;
    }

    void Update()
    {
        if (_notMove)
        {
            //スクリーン座標をワールド座標に変換
            worldPos = new Vector3(4.0f, 0.0f, 10f);
            transform.position = worldPos;
        }
        else
        {
            //マウス座標の取得
            mousePos = Input.mousePosition;
            //スクリーン座標をワールド座標に変換
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            //ワールド座標を自身の座標に設定
            transform.position = worldPos;
        }

        _spriteRenderer.material.SetFloat("_BeforeColorAmount", Mathf.Clamp(_garbageValue, -1.0f, 1.0f));
    }

    public float GarbageValue
    {
        get { return _garbageValue; }
        set { _garbageValue = value; }
    }
}