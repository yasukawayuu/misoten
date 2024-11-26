using UnityEngine;

public class CircleFadeController : MonoBehaviour
{
    public Material circleFadeMaterial; // この変数に材質をリンクする
    private float maskProgress = 0f; // 円形の進行状況
    private bool isExpanding = true; // 円形が拡大しているか縮小しているかを制御するフラグ

    public float speed = 0.5f; // 円形の拡大/縮小速度

    void Start()
    {
        // 材質がリンクされている場合、_MaskProgress の初期値を設定
        if (circleFadeMaterial != null)
        {
            circleFadeMaterial.SetFloat("_MaskProgress", maskProgress);
        }
    }

    void Update()
    {
        // スペースキーを押したとき、状態を変更（拡大から縮小、またはその逆）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isExpanding = !isExpanding;
        }

        // 円形の拡大または縮小を制御
        if (isExpanding)
        {
            maskProgress += speed * Time.deltaTime; // 拡大処理
            if (maskProgress > 1f) maskProgress = 1f; // 最大値は1
        }
        else
        {
            maskProgress -= speed * Time.deltaTime; // 縮小処理
            if (maskProgress < 0f) maskProgress = 0f; // 最小値は0
        }

        // 材質の _MaskProgress パラメータを更新
        if (circleFadeMaterial != null)
        {
            circleFadeMaterial.SetFloat("_MaskProgress", maskProgress);
        }
    }
}
