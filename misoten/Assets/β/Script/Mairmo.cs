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
    private bool _isKing = true;

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


        // 王冠処理
        if(_isKing && !_crawn.activeSelf)
        { 
            _crawn.SetActive(true);

        }
        if(!_isKing && _crawn.activeSelf)
        {
            _crawn.SetActive(false);
        }
    }

    protected virtual void Move(){}
}
