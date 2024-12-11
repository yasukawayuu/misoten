using UnityEngine;

public class CircleFadeController : MonoBehaviour
{
    public Material circleFadeMaterial; // 円形フェードのマテリアル
    private float maskProgress = 0f; // マスクの現在の進行度
    private bool isExpanding = true; // マスクが拡大中か、trueで拡大、falseで縮小

    public float speed = 0.5f; // マスクの拡大/縮小速度

    void Start()
    {
        // マテリアルの _MaskProgress プロパティを 0 に初期化
        if (circleFadeMaterial != null)
        {
            circleFadeMaterial.SetFloat("_MaskProgress", maskProgress);
        }
    }

    void Update()
    {
        // スペースキーを押した時に拡大/縮小状態を切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isExpanding = !isExpanding;
        }

        // 状態に応じてマスクの進行度を更新
        if (isExpanding)
        {
            maskProgress += speed * Time.deltaTime; // 拡大
            if (maskProgress > 1f) maskProgress = 1f; // 最大値を1に制限
        }
        else
        {
            maskProgress -= speed * Time.deltaTime; // 縮小
            if (maskProgress < 0f) maskProgress = 0f; // 最小値を0に制限
        }

        // マテリアルの _MaskProgress プロパティを更新
        if (circleFadeMaterial != null)
        {
            circleFadeMaterial.SetFloat("_MaskProgress", maskProgress);
        }
    }
}
