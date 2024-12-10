using UnityEngine;

public class RippleEffectController : MonoBehaviour
{
    public GameObject ripplePrefab;  // リップル（波紋）エフェクトのプレハブ

    void Update()
    {
        // マウスのクリックまたはタッチ操作を検出
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // マウスまたはタッチ入力のスクリーン座標を取得
            Vector3 screenPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;

            // スクリーン座標をワールド座標に変換
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;  // 2D空間での使用のため、Z座標を0に固定

            // リップルエフェクトのインスタンスを生成
            GameObject rippleInstance = Instantiate(ripplePrefab, worldPos, Quaternion.identity);

            // リップルエフェクトのアニメーションを再生
            Animator animator = rippleInstance.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.Play("RippleAnimation");  // リップルアニメーションの再生
            }

            // リップルエフェクトを一定時間後に破棄
            Destroy(rippleInstance, 0.5f);  // 0.5秒後にオブジェクトを削除
        }
    }
}
