using UnityEngine;

public class PlayerCollisionSEWithDirection : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound; // 衝突音
    [SerializeField] private float maxHearingRange = 10f; // 最大聴取範囲
    [SerializeField] private float maxVolume = 1f; // 最大音量
    [SerializeField] private float attenuationFactor = 1f; // 音の減衰係数（デフォルトは1）
    [SerializeField] private bool useTrigger = false; // トリガーモードを使用するか
    [SerializeField] private float triggerSoundVolume = 1f; // トリガー音の音量

    private AudioSource audioSource;

    void Start()
    {
        // AudioSourceの初期化
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceの初期化に失敗しました！");
            return;
        }

        audioSource.playOnAwake = false; // 自動再生しない
        audioSource.spatialBlend = 1f;  // 3D音（2Dの場合も使用可能）
        audioSource.clip = collisionSound;

        if (collisionSound == null)
        {
            Debug.LogError("AudioClipが設定されていません。InspectorでcollisionSoundを設定してください！");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger) // トリガーモードでない場合にのみ有効
        {
            PlayCollisionSound(collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (useTrigger) // トリガーモードである場合にのみ有効
        {
            PlayCollisionSound(other);
        }
    }

    /// <summary>
    /// 衝突音を再生する
    /// </summary>
    /// <param name="other">衝突したオブジェクト</param>
    private void PlayCollisionSound(Collider2D other)
    {
        if (collisionSound == null)
        {
            Debug.LogWarning("AudioClipが設定されていません。音を再生できません！");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSourceが正しく初期化されていません！");
            return;
        }

        // 衝突物の位置から距離を計算
        float distance = Vector2.Distance(transform.position, other.transform.position);
        if (distance > maxHearingRange) return; // 範囲外は再生しない

        // 距離に応じて音量を調整（距離が遠いほど音量が小さくなる）
        float adjustedDistance = Mathf.Pow(distance / maxHearingRange, attenuationFactor); // 減衰係数を使用して調整
        float volume = Mathf.Clamp(1f - adjustedDistance, 0f, maxVolume);

        // トリガー音の音量を設定
        audioSource.volume = volume * triggerSoundVolume;

        // 衝突点から音の方向を計算
        Vector2 directionToPlayer = (transform.position - other.transform.position).normalized;

        // 音の左右バランスを設定（音の方向をシミュレート）
        audioSource.panStereo = directionToPlayer.x; // X軸の位置に基づいて左右バランスを制御

        // 音を再生
        audioSource.Play();

        Debug.Log($"音を再生: 衝突オブジェクト={other.name}, 音量={audioSource.volume}, 距離={distance}, 減衰係数={attenuationFactor}");
    }
}
