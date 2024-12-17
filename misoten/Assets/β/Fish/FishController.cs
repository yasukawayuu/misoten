using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishController : MonoBehaviour
{
    private float _speed = 5.0f; // 魚の移動速度
    private float _limit = 0;

    private void Start()
    {
        
        switch(SceneManager.GetActiveScene().name)
        {
            case "Title":
                _limit = 10.0f;
                break;
            case "Marimo.io":
                _limit = 200.0f;
                break;
        }
    }

    private void Update()
    {
        // 魚をターゲット位置に向けて移動
        transform.position += transform.up * Time.deltaTime * _speed;


        // 目標位置が画面内にあるかを判定
        if (transform.position.x < -_limit || transform.position.x > _limit || transform.position.y < -_limit || transform.position.y > _limit)
        {
             // 画面外の場合、魚を破壊
             Destroy(gameObject);
        }

    }
}
