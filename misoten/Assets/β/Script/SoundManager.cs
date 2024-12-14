using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource _bgmSource;
    private AudioSource _seSource3D;
    private IDictionary<string, AudioSource> _seSources2D = new Dictionary<string, AudioSource>();


    [SerializeField] private AudioClip _bgm;
    [SerializeField] private AudioClip _clean;
    [SerializeField] private AudioClip _charge;
    [SerializeField] private AudioClip _recovery;

    private void Start()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.clip = _bgm;
        _bgmSource.loop = true;
        _bgmSource.Play();

        //SEÇ3DÉTÉEÉìÉhÇ…ê›íË
        _seSource3D = gameObject.AddComponent<AudioSource>();
        _seSource3D.spatialBlend = 1.0f;
        _seSource3D.rolloffMode = AudioRolloffMode.Linear;
        _seSource3D.minDistance = 1.0f;
        _seSource3D.maxDistance = 50.0f;

        _seSources2D["charge"] = gameObject.AddComponent<AudioSource>();
        _seSources2D["charge"].clip = _charge;
        _seSources2D["clean"] = gameObject.AddComponent<AudioSource>();
        _seSources2D["clean"].clip = _clean;
        _seSources2D["recovery"] = gameObject.AddComponent<AudioSource>();
        _seSources2D["recovery"].clip = _recovery;
    }

    private void Update()
    {
        if (GameSceneManager.Instance)
            _bgmSource.volume = GameSceneManager.Instance.MaskProgress;
    }

    public void PlayBGM(AudioClip clip)
    {
        _bgmSource.Stop();
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void PlaySE3D(Vector2 position,AudioClip clip) 
    {
        _seSource3D.transform.position = position;
        _seSource3D.PlayOneShot(clip);
    }

    public void PlaySE2D(string clip,float pitch = 1.0f)
    {
        _seSources2D[clip].pitch = pitch;

        if(!_seSources2D[clip].isPlaying)
            _seSources2D[clip].PlayOneShot(_seSources2D[clip].clip);
    }

}
