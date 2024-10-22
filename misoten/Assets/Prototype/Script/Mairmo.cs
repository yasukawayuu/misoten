using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marimo : MonoBehaviour
{
    protected int _point = 0;
    protected string _name = "";

    public int Point
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
