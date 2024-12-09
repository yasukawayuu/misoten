using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs; // 複数の魚のプレハブを格納
    public Camera mainCamera; // メインカメラ
    public float spawnInterval = 3f; // 魚の生成間隔
    private GameObject lastSpawnedFishPrefab = null; // 最後に生成された魚のプレハブ（最初のプレハブかどうかを判断）

    private void Start()
    {
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
        // カメラの四隅のスクリーン座標を取得
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // ランダムに位置を決定：画面外のどの辺から生成するか（左、右、上、下）
        Vector3 spawnPosition = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;
        int side = Random.Range(0, 4); // 0:左, 1:右, 2:上, 3:下

        switch (side)
        {
            case 0: // 左側から生成、右側に消える
                spawnPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                targetPosition = new Vector3(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 1: // 右側から生成、左側に消える
                spawnPosition = new Vector3(screenTopRight.x + 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                targetPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(screenBottomLeft.y, screenTopRight.y), 0);
                break;
            case 2: // 上側から生成、下側に消える
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1, 0);
                targetPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1, 0);
                break;
            case 3: // 下側から生成、上側に消える
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - 1, 0);
                targetPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + 1, 0);
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
                Vector3 rowStartPosition = spawnPosition + new Vector3(row * 1.5f, 0, 0); // 各行の間隔（1.5fは例示値、調整可能）

                for (int col = 0; col < maxColumns; col++)
                {
                    if ((row * maxColumns + col) >= numberOfFishToSpawn) break; // 必要な数を超えないようにする

                    // 重なりを避けるためにランダムに位置をオフセット
                    Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                    Vector3 fishPosition = rowStartPosition + new Vector3(col * 1.5f, 0, 0) + offset; // 各魚の位置
                    GameObject fish = Instantiate(selectedFishPrefab, fishPosition, Quaternion.identity);
                    FishController fishController = fish.GetComponent<FishController>();
                    if (fishController != null)
                    {
                        // 目標位置を設定
                        fishController.SetTargetPosition(targetPosition);
                    }
                }
            }
        }
        else
        {
            // 1匹だけの魚を生成
            GameObject fish = Instantiate(selectedFishPrefab, spawnPosition, Quaternion.identity);
            FishController fishController = fish.GetComponent<FishController>();
            if (fishController != null)
            {
                // 目標位置を設定
                fishController.SetTargetPosition(targetPosition);
            }
        }

        // 最後に生成された魚のプレハブを更新
        lastSpawnedFishPrefab = selectedFishPrefab;
    }
}
