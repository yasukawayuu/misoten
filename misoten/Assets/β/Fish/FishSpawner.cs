using System.Collections;
using TMPro;
using UnityEngine;
public class FishSpawner : MonoBehaviour
{
    [SerializeField]private GameObject[] fishPrefabs; // 複数の魚のプレハブを格納
    [SerializeField]private float spawnInterval = 3f; // 魚の生成間隔
    [SerializeField] private float _vertices;
    private GameObject lastSpawnedFishPrefab = null; // 最後に生成された魚のプレハブ（最初のプレハブかどうかを判断）
   

    private void Start()
    {
        for(int i = 0;i < 10;i++)
        {
            SpawnFish();
        }
        // 魚を定期的に生成するコルーチンを開始
        StartCoroutine(SpawnFishCoroutine());
    }

    private IEnumerator SpawnFishCoroutine()
    {
        while (true)
        {
            // 魚を生成
            SpawnFish();
            // 次の生成まで待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnFish()
    {
        // マップの四頂点座標を取得
        Vector2 screenBottomLeft = new Vector3(-_vertices, _vertices);
        Vector2 screenTopRight = new Vector3(_vertices, -_vertices);

        // ランダムに位置を決定：マップのどの辺から生成するか（左、右、上、下）
        Vector2 spawnPosition = Vector3.zero;
        Vector2 targetPosition = Vector3.zero;
        int side = Random.Range(0, 4); // 0:左, 1:右, 2:上, 3:下

        switch (side)
        {
            case 0: // 左側から生成、右側に消える
                spawnPosition = new Vector2(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y));
                targetPosition = new Vector2(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y));
                break;
            case 1: // 右側から生成、左側に消える
                spawnPosition = new Vector2(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y));
                targetPosition = new Vector2(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y));
                break;
            case 2: // 上側から生成、下側に消える
                spawnPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1);
                targetPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1);
                break;
            case 3: // 下側から生成、上側に消える
                spawnPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1);
                targetPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1);
                break;
        }

        // 魚のプレハブをランダムに選択
        GameObject selectedFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];

        // 最初のプレハブが生成される場合、10から20匹の魚を生成
        if (selectedFishPrefab == fishPrefabs[0] && lastSpawnedFishPrefab != selectedFishPrefab)
        {
            int numberOfFishToSpawn = Random.Range(10, 20); // 10から20匹の魚をランダムに生成
            int maxColumns = 3; // 1行あたり最大3匹の魚
            int rows = Mathf.CeilToInt(numberOfFishToSpawn / (float)maxColumns); // 必要な行数を計算

            // 複数の魚を生成
            for (int row = 0; row < rows; row++)
            {
                // 各行の開始位置を計算
                Vector2 rowStartPosition = spawnPosition + new Vector2(row * 1.5f, 0); // 各行の間隔（1.5fは例示値、調整可能）

                for (int col = 0; col < maxColumns; col++)
                {
                    if ((row * maxColumns + col) >= numberOfFishToSpawn) break; // 必要な数を超えないようにする

                    // 重なりを避けるためにランダムに位置をオフセット
                    Vector2 offset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    Vector2 fishPosition = rowStartPosition + new Vector2(col * 1.5f, 0) + offset; // 各魚の位置
                    Instantiate(selectedFishPrefab, fishPosition, SetFishRotation(targetPosition, fishPosition));
                   
                }
            }
        }
        else
        {
            // 1匹だけの魚を生成
            GameObject fish = Instantiate(selectedFishPrefab, spawnPosition, SetFishRotation(targetPosition, spawnPosition));
            float scale = Random.Range(0.5f, 2.0f);
            fish.transform.localScale = new Vector3( scale,scale,scale);
        }

        // 最後に生成された魚のプレハブを更新
        lastSpawnedFishPrefab = selectedFishPrefab;
    }

    private Quaternion SetFishRotation(Vector2 target, Vector2 spawn)
    {
        Vector2 direction = target - spawn;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
        return rotation;
    }


}

