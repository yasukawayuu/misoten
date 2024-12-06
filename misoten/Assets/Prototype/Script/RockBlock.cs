using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 岩スクリプト
public class RockBlock : MonoBehaviour
{
    public GameObject[] rockPrefabs;   // 格納配列
    public int rockCount = 10;   // 岩の数
    public Vector2 spawnRange = new Vector2(10f, 30f);   // ランダムな位置の範囲

    void Start()
    {
        GenerateRocks();
    }

    // 岩生成関数
    void GenerateRocks()
    {
        for (int i = 0; i < rockCount; i++)
        {
            // ランダムにプレハブを選択
            GameObject rockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];

            // ランダムな位置を計算
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x),  // 横方向ランダム
                Random.Range(-spawnRange.y, spawnRange.y),  // 縦方向ランダム
                0f                                         // Z軸は2Dでは固定
            );

            // 岩を生成
            Instantiate(rockPrefab, randomPosition, Quaternion.identity);
        }
    }

    // Playerとの当たり判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the rock!");
        }
    }
}
