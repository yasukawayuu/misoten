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
        var collision = this.GetComponent<ParticleSystem>().collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.mode = ParticleSystemCollisionMode.Collision3D;

    }

    private void FixedUpdate()
    {

        
       
    }

    private void TrackingMousePosition()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _rb2D.MovePosition(transform.position + direction * _speed * Time.fixedDeltaTime);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePosition;
    }
}
