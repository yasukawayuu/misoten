using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Marimo : MonoBehaviour
{
    [CustomLabel("ポイント")]
    [SerializeField] protected float _point = 1.0f;

    [CustomLabel("現在の汚染ポイント")]
    [SerializeField] protected float _garbageValue = 0.0f;

    [CustomLabel("汚染ポイント上限")]
    [SerializeField] protected int _maxGarbageValue = 5;

    [CustomLabel("王冠")]
    [SerializeField] protected GameObject _crawn;
    private bool _isKing = false;

    public float Point
    {
        get { return _point; }
        set { _point = value; }
    }

    public bool King
    {
        get { return _isKing; }
        set { _isKing = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(currentPos.x, -100.0f, 100.0f);
        currentPos.y = Mathf.Clamp(currentPos.y, -100.0f, 100.0f);

        transform.position = currentPos;


        // 王冠処理
        if(_isKing)
        { 
            if (!_crawn.activeSelf)
            {
                _crawn.SetActive(true);
            }
            _crawn.transform.localPosition = new Vector3(0.07f + _point * 0.2f, 0.07f + _point * 0.2f, 0.0f);
        }
        if(!_isKing && _crawn.activeSelf)
        {
            _crawn.SetActive(false);
        }
    }

    protected virtual void Move(){}
}
