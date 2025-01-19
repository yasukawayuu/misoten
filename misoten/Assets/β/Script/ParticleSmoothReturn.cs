using UnityEngine;

enum ParticleState
{
    Idle,
    Play,
    Return,
};

public class ParticleSmoothReturn : MonoBehaviour
{
    public ParticleSystem particleSystem; // 対象のParticle System
    public float maxDistance = 5f; // 中心からの最大距離
    public Vector3 centerPosition = Vector3.zero; // 中心座標
    public float returnSpeed = 1f; // 中心に戻る速度

    private ParticleState state = ParticleState.Idle;

    private ParticleSystem.Particle[] particles;

    void Start()
    {
        centerPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (particleSystem == null)
            return;

        // パーティクルの配列を取得または初期化
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }

        // 現在のパーティクル情報を取得
        int particleCount = particleSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            // パーティクルと中心座標の距離を計算
            float distance = Vector3.Distance(particles[i].position, centerPosition);

            // 距離が一定以上の場合、中心に向かって徐々に戻す
            if (distance > maxDistance)
            {
                // 現在の位置と中心位置の間を補間
                particles[i].position = Vector3.Lerp(
                    particles[i].position, // 現在の位置
                    centerPosition,        // 中心座標
                    returnSpeed * Time.deltaTime // 補間速度
                );

                state = ParticleState.Return;
                var ex = particleSystem.externalForces;
                ex.enabled = false;

                // 必要に応じて速度を調整（加速効果）
                Vector3 directionToCenter = (centerPosition - particles[i].position).normalized;
                particles[i].velocity = directionToCenter * returnSpeed;
            }

            if (distance < 1.2f && state == ParticleState.Return)
            {
                state = ParticleState.Idle;
                var ex = particleSystem.externalForces;
                ex.enabled = true;
                particles[i].velocity *= 0.001f;
            }
        }

        // 更新されたパーティクル情報を反映
        particleSystem.SetParticles(particles, particleCount);
    }
}
