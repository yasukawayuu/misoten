using UnityEngine;

public class FishController : MonoBehaviour
{
    public float speed = 2f; // 魚の移動速度
    public float angleOffset = -90f; // 角度のオフセット。Unityエディタで調整可能
    public float destroyDistanceMultiplier = 3f; // 破壊位置の倍率
    public Camera mainCamera; // カメラ、視野を判定するために使用

    private Vector3 targetPosition;

    private void Start()
    {
        // Inspectorでカメラが割り当てられていない場合、シーン内のMain Cameraを自動で取得
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 主カメラを取得
        }
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;

        // 方向を計算して回転を調整
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 角度を計算
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset); // 角度オフセットを加える
    }

    private void Update()
    {
        // 魚をターゲット位置に向けて移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 魚がターゲット位置に到達したかチェック
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // カメラの視野範囲を取得
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);

            // 2DゲームではカメラのZ値は固定されるべき
            screenPosition.z = 0f; // z値を0に設定。xとy軸のみが重要

            // 目標位置が画面内にあるかを判定
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                // 画面外の場合、魚を破壊
                Destroy(gameObject);
            }
            else
            {
                // 目標位置が視野内にあれば、ターゲット位置を更新
                // 破壊位置のオフセットを計算
                Vector3 direction = targetPosition - transform.position;
                Vector3 offsetPosition = direction.normalized * destroyDistanceMultiplier;

                // ターゲット位置を延長
                targetPosition = targetPosition + offsetPosition;
                SetTargetPosition(targetPosition); // ターゲット位置と角度を更新
            }
        }
    }
}
