using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Marimo : MonoBehaviour
{
    [SerializeField] protected float _point = 1.0f;
    [SerializeField] protected float _garbageValue = 0.0f;
    [SerializeField] protected int _maxGarbageValue = 5;
    [SerializeField] protected string _name = "";

    public float Point
    {
        get { return _point; }
    }

    public string Name
    {
        get { return _name; }
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
