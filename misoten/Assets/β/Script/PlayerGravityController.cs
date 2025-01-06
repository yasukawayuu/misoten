using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    private Player _player;

    [SerializeField] private Transform _marimoBody;
    [SerializeField] private float _baseGravityScale = 1.2f;
    [SerializeField] private GameObject _gravityForceField;
    [SerializeField] private float _baseCollisionScale = 0.2f;
    [SerializeField] private GameObject _gravityCollision;

    static private Vector3 _base = new Vector3(1.0f, 1.0f, 1.0f);

    public void GravitySetteing(Player player)
    {
        _player = player;
    }

    void Start()
    {
        _gravityForceField.transform.localScale = _base * _baseGravityScale;
        _gravityCollision.transform.localScale = _base * _baseCollisionScale;
    }

    void Update()
    {
        this.transform.position = _marimoBody.position;


        if (_player.Point < 20)
        {
            _gravityForceField.transform.localScale = _base * (_baseGravityScale - ((0.3f + _baseGravityScale - 1.0f) * (_player.Point / 20.0f)));
            _gravityCollision.transform.localScale = _base * (_baseCollisionScale + (0.2f * (_player.Point / 20.0f)));
        }
        else
        {
            _gravityForceField.transform.localScale = _base * 0.7f;
            _gravityCollision.transform.localScale = _base * (_baseCollisionScale + 0.2f);
        }
    }
}
