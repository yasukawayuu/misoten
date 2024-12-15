using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource _bgmSource;
    private AudioSource _seSource3D;

    private IDictionary<string, AudioSource> _seSources2D = new Dictionary<string, AudioSource>();
    [SerializeField] private AudioClip[] _seClips = null;

    [SerializeField] private AudioClip _bgm;


    private void Start()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.clip = _bgm;
        _bgmSource.loop = true;
        _bgmSource.Play();

        //SEを3Dサウンドに設定
        _seSource3D = gameObject.AddComponent<AudioSource>();
        _seSource3D.spatialBlend = 1.0f;
        _seSource3D.rolloffMode = AudioRolloffMode.Linear;
        _seSource3D.minDistance = 1.0f;
        _seSource3D.maxDistance = 50.0f;

        //インスペクター上で追加した音声ファイルをファイル名で各AudioSourceに追加する
        for(int i = 0;i < _seClips.Length;i++)
        {
            _seSources2D[_seClips[i].name] = gameObject.AddComponent<AudioSource>();
            _seSources2D[_seClips[i].name].clip = _seClips[i];
        }
    }

    private void Update()
    {
        if (GameSceneManager.Instance && GameSceneManager.Instance.Panel.activeSelf)
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume = GameSceneManager.Instance.MaskProgress;
            }
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        _bgmSource.Stop();
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    /// <summary>
    /// どこで音がなっているかを知るためのPositon
    /// </summary>
    /// <param name="position"></param>
    /// <param name="clip"></param>
    public void PlaySE3D(Vector2 position,AudioClip clip) 
    {
        _seSource3D.transform.position = position;
        _seSource3D.PlayOneShot(clip);
    }

    /// <summary>
    /// なんの音鳴らすのと音の速さを調整
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="pitch"></param>
    public void PlaySE2D(string clip,float pitch = 1.0f)
    {
        _seSources2D[clip].pitch = pitch;

        if(!_seSources2D[clip].isPlaying)
            _seSources2D[clip].PlayOneShot(_seSources2D[clip].clip);
    }
    
    public void SetVolume(float volume)
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        for(int i = 0; i < audioSources.Length; i++) 
        {
            audioSources[i].volume = volume;
        }
    }

}
