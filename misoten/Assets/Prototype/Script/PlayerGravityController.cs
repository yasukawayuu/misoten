using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _baseGravityScale = 0.88f;
    [SerializeField] private GameObject _gravityForceField;
    [SerializeField] private float _baseCollisionScale = 0.15f;
    [SerializeField] private GameObject _gravityCollision;

    static private Vector3 _base = new Vector3(1.0f, 1.0f, 1.0f);

    public void GravitySetteing(Player player)
    {
        _player = player;
    }

    void Update()
    {
        Vector3 scale = _base * _baseGravityScale;
        _gravityForceField.transform.localScale = scale;

        scale = _base * _baseCollisionScale;
        _gravityCollision.transform.localScale = scale;
    }
}
