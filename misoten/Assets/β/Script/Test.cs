using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Test : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] float _speed;
    [SerializeField] Transform _target;

    void Start()
    {
        
    }

    void Update()
    {
        

    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _rb2D.MovePosition(transform.position + direction * _speed * Time.fixedDeltaTime);
        }
       
    }

    private void TrackingMousePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePosition;
    }
}
