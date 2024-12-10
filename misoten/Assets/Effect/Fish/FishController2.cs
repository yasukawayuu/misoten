using UnityEngine;

public class FishController2 : MonoBehaviour
{
    public float speed = 2f; // 魚の基礎移動速度
    public float angleOffset = -90f; // 角度オフセット
    public float destroyDistanceMultiplier = 3f; // 破壊位置の倍率
    public Camera mainCamera; // カメラ、視野を判定するために使用

    [Header("ランダム化設定")]
    public Vector2 sizeRange = new Vector2(0.5f, 2f); // 魚のサイズ範囲
    public Vector2 speedRange = new Vector2(1f, 5f); // 魚の速度範囲
    public float largeFishSizeThreshold = 1.5f; // 大きな魚のサイズ閾値

    private Vector3 targetPosition;

    private void Start()
    {
        // mainCameraが設定されていない場合、シーン内のメインカメラを取得
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // 魚のサイズと速度をランダムに設定
        RandomizeFish();

        // 初期方向の設定
        // SetInitialDirection();
        SetTargetPosition(targetPosition); // ターゲット位置と角度を更新
    }

    // 魚の移動先位置を設定
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

        // 目標位置に向かう方向を計算し、回転を調整
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 角度を計算
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // 角度オフセットを加える
    }

    // 魚のサイズと速度をランダムに設定
    private void RandomizeFish()
    {
        bool largeFishExists = false;
        FishController2[] fishes = FindObjectsOfType<FishController2>(); // シーン内のすべてのFishController2を取得
        foreach (FishController2 fish in fishes)
        {
            // 大きな魚が存在するか確認
            if (fish.transform.localScale.x >= largeFishSizeThreshold)
            {
                largeFishExists = true;
                break;
            }
        }

        // 魚のサイズをランダムに設定
        float randomSize = Random.Range(sizeRange.x, sizeRange.y);

        // 大きな魚が存在し、ランダムに選ばれたサイズが閾値以上の場合、サイズを小さく調整
        if (largeFishExists && randomSize >= largeFishSizeThreshold)
        {
            randomSize = Random.Range(sizeRange.x, largeFishSizeThreshold);
        }

        // 魚のスケールを設定
        transform.localScale = new Vector3(randomSize, randomSize, 1f);

        // サイズに基づいて速度を調整
        float sizeFactor = (randomSize - sizeRange.x) / (sizeRange.y - sizeRange.x);
        speed = Mathf.Lerp(speedRange.y, speedRange.x, sizeFactor); // サイズに応じて速度をリニア補間
    }

    private void Update()
    {
        // 魚をターゲット位置に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 目標位置に到達したかどうかを確認
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ターゲット位置をスクリーン座標に変換
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            screenPosition.z = 0f; // 2Dゲームなのでz値は無視

            // 目標位置が画面内にあるかを判定
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                // 画面外に出た場合、魚を破壊
                Destroy(gameObject);
            }
            else
            {
                // 目標位置が画面内にある場合、ターゲット位置を更新
                // 目標位置を延長するためのオフセットを計算
                Vector3 direction = targetPosition - transform.position;
                Vector3 offsetPosition = direction.normalized * destroyDistanceMultiplier;

                // ターゲット位置を延長
                targetPosition = targetPosition + offsetPosition;
                SetTargetPosition(targetPosition); // ターゲット位置と角度を更新
            }
        }
    }
}
