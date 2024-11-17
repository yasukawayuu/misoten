using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageParticleController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private List<ParticleSystem.Particle> _enterParticles = new List<ParticleSystem.Particle>();

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        Invoke(nameof(StopEmitting), 5.0f);
    }

    void StopEmitting()
    {
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    void OnParticleTrigger()
    {
        // このフレームでトリガー条件に一致するパーティクルを取得
        int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);

        // 衝突判定のための探索範囲
        float detectionRadius = 0.2f; // パーティクルの周囲の判定半径

       
        // トリガーに侵入したパーティクルを処理
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle particle = _enterParticles[i];

            // パーティクルのワールド座標を取得
            Vector3 particlePosition = particle.position;

            // 2DのColliderを検出（OverlapCircleを使う）
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(particlePosition, detectionRadius);

            
            foreach (var collider in hitColliders)
            {
                // Playerタグを持つオブジェクトの場合の処理例
                if (collider.CompareTag("Player"))
                {
                    collider.gameObject.GetComponent<Player>().EatGarbage();
                }
            }

            // パーティクルのライフタイムを終了
            particle.remainingLifetime = -1.0f;
            _enterParticles[i] = particle;
        }

        // 変更したパーティクルを再設定
        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);

        // 次のフレームで削除確認
        if (_particleSystem.particleCount == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
