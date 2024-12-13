using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource _bgmSource;
    private AudioSource _seSource3D;
    private AudioSource _seSource2D;
    
    [SerializeField] private AudioClip _bgm;

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

        _seSource2D = gameObject.AddComponent<AudioSource>();
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

    public void PlaySE2D(AudioClip clip,float pitch = 1.0f)
    {
        _seSource2D.pitch = pitch;

        if(!_seSource2D.isPlaying)
            _seSource2D.PlayOneShot(clip);
    }

}
