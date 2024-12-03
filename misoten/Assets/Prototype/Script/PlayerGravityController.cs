using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    private Player _player;

    [SerializeField] private Transform _marimoBody;
    [SerializeField] private float _baseGravityScale = 1.5f;
    [SerializeField] private GameObject _gravityForceField;
    [SerializeField] private float _baseCollisionScale = 0.2f;
    [SerializeField] private GameObject _gravityCollision;

    static private Vector3 _base = new Vector3(1.0f, 1.0f, 1.0f);

    public void GravitySetteing(Player player)
    {
        _player = player;
        Debug.Log("test");
    }

    void Update()
    {
        this.transform.position = _marimoBody.position;

        Vector3 point = new Vector3(_player.Point * 0.2f, _player.Point * 0.2f, _player.Point * 0.2f);
        Vector3 scale = _base * _baseGravityScale + point;
        _gravityForceField.transform.localScale = scale;

        scale = _base * _baseCollisionScale + point;
        _gravityCollision.transform.localScale = scale;

        //Debug.Log(_gravityForceField.transform.localScale);
    }
}
