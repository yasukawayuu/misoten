using UnityEngine;

public class RippleEffectController : MonoBehaviour
{
    public GameObject ripplePrefab;  // 波紋エフェクトのプレハブを参照

    void Update()
    {
        // マウス左クリック または タッチを検出
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // スクリーン位置（マウスまたはタッチ位置）をワールド座標に変換
            Vector3 screenPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;  // 2D空間内の位置に設定

            // 波紋エフェクトをインスタンス化
            GameObject rippleInstance = Instantiate(ripplePrefab, worldPos, Quaternion.identity);

            // 波紋アニメーションを再生
            Animator animator = rippleInstance.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.Play("RippleAnimation");  // 設定したアニメーションを再生
            }

            // アニメーション終了後、インスタンスを破棄
            Destroy(rippleInstance, 0.5f);  // アニメーションの長さが0.5秒と仮定
        }
    }
}
