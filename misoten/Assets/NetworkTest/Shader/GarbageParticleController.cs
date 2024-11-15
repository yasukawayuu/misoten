using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageParticleController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private List<ParticleSystem.Particle> _enterParticles = new List<ParticleSystem.Particle>();
    [SerializeField] private GameObject _player;
    private MetaBallTestCharacter _metaBallTestCharacter;

    void Start()
    {
        if (_player.GetComponent<MetaBallTestCharacter>() == null)
        {
            Debug.LogWarning("PlayerにMetaBallTestCharacterが存在しません");
        }
        else
        {
            _metaBallTestCharacter = _player.GetComponent<MetaBallTestCharacter>();
        }

        _particleSystem = GetComponent<ParticleSystem>();
        Invoke(nameof(StopEmitting), 5.0f);
    }

    void StopEmitting()
    {
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    void OnParticleTrigger()
    {
        // このフレームのトリガーの条件に一致するパーティクルを取得
        int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);


        // トリガーに侵入したパーティクルを走査
        for (int i = 0; i < numEnter; i++)
        {
            _metaBallTestCharacter.GarbageValue -= 0.05f;

            ParticleSystem.Particle p = _enterParticles[i];
            p.remainingLifetime = -1.0f;
            _enterParticles[i] = p;
        }

        // 変更したパーティクルをパーティクルシステムに再割り当て
        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enterParticles);
    }
}
