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


    public float Point
    {
        get { return _point; }
        set { _point = value; }
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
    }

    protected virtual void Move(){}

}
