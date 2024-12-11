using UnityEngine;

public class FishController : MonoBehaviour
{
    private float _speed = 5.0f; // 魚の移動速度

    private void Start()
    {

    }

    private void Update()
    {
        // 魚をターゲット位置に向けて移動
        transform.position += transform.up * Time.deltaTime * _speed;


        // 目標位置が画面内にあるかを判定
        if (transform.position.x < -200 || transform.position.x > 200 || transform.position.y < -200 || transform.position.y > 200)
        {
             // 画面外の場合、魚を破壊
             Destroy(gameObject);
        }

    }
}
