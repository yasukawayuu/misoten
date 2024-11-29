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
        //// このフレームでトリガー条件に一致するパーティクルを取得
        //int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);

        //// 衝突判定のための探索範囲
        //float detectionRadius = 0.2f; // パーティクルの周囲の判定半径

        //// トリガーに侵入したパーティクルを処理
        //for (int i = 0; i < numEnter; i++)
        //{
        //    ParticleSystem.Particle particle = _enterParticles[i];

        //    // パーティクルのワールド座標を取得
        //    Vector3 particlePosition = particle.position;

        //    // 2DのColliderを検出（OverlapCircleを使う）
        //    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(particlePosition, detectionRadius);

        //    foreach (var collider in hitColliders)
        //    {
        //        // Playerタグを持つオブジェクトの場合の処理例
        //        if (collider.gameObject.GetComponent<Player>())
        //        {
        //            collider.gameObject.GetComponent<Player>().EatGarbage();
        //        }
        //    }

        //    // パーティクルのライフタイムを終了
        //    particle.remainingLifetime = -1.0f;
        //    _enterParticles[i] = particle;
        //}

        //// 変更したパーティクルを再設定
        //_particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);

        // トリガー条件に一致するパーティクルを取得
        int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);

        // 探索範囲
        float detectionRadius = 0.2f; // パーティクルの周囲の判定半径

        // トリガーに侵入したパーティクルを処理
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle particle = _enterParticles[i];

            // パーティクルのワールド座標を取得
            Vector3 particlePosition = particle.position;

            // 全てのオブジェクトとの衝突判定を行う
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(particlePosition, detectionRadius);

            foreach (var collider in hitColliders)
            {
                // Player タグを持つオブジェクトの例
                if (collider.gameObject.tag == "Player")
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

    void OnParticleCollision(GameObject other)
    {
        // 衝突したオブジェクトが Player の場合のみ処理
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.EatGarbage();

        // 衝突イベントを取得
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int collisionCount = _particleSystem.GetCollisionEvents(other, collisionEvents);

        // パーティクルを取得して処理
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_particleSystem.particleCount];
        _particleSystem.GetParticles(particles);

        for (int i = 0; i < collisionCount; i++)
        {
            Debug.Log("collisionCount " + collisionCount);

            Vector3 collisionPosition = collisionEvents[i].intersection;

            // パーティクルを衝突位置に基づいて検索
            for (int j = 0; j < particles.Length; j++)
            {
                // パーティクルの位置が衝突位置に近ければ削除
                if (Vector3.Distance(particles[j].position, collisionPosition) < 0.1f) // しきい値は適宜調整
                {
                    particles[j].remainingLifetime = -1.0f;
                }
            }
        }

        // パーティクルを再設定
        _particleSystem.SetParticles(particles);

        // 次のフレームで削除確認
        if (_particleSystem.particleCount == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
