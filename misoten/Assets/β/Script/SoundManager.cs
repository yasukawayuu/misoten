using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource _bgmSource;

    private IDictionary<string, AudioSource> _seSources2D = new Dictionary<string, AudioSource>();
    private IDictionary<string, AudioSource> _seSources3D = new Dictionary<string, AudioSource>();

    [SerializeField] private AudioClip[] _seClips2D = null;
    [SerializeField] private AudioClip[] _seClips3D = null;

    [SerializeField] private AudioClip _bgm;


    private void Start()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.clip = _bgm;
        _bgmSource.loop = true;
        _bgmSource.Play();
       
        //インスペクター上で追加した音声ファイルをファイル名で各AudioSourceに追加する
        for (int i = 0; i < _seClips3D.Length; i++)
        {
            _seSources3D[_seClips3D[i].name] = gameObject.AddComponent<AudioSource>();
            _seSources3D[_seClips3D[i].name].spatialBlend = 1.0f;
            _seSources3D[_seClips3D[i].name].rolloffMode = AudioRolloffMode.Linear;
            _seSources3D[_seClips3D[i].name].minDistance = 1.0f;
            _seSources3D[_seClips3D[i].name].maxDistance = 50.0f;
            _seSources3D[_seClips3D[i].name].clip = _seClips3D[i];
        }

        //インスペクター上で追加した音声ファイルをファイル名で各AudioSourceに追加する
        for (int i = 0;i < _seClips2D.Length;i++)
        {
            _seSources2D[_seClips2D[i].name] = gameObject.AddComponent<AudioSource>();
            _seSources2D[_seClips2D[i].name].clip = _seClips2D[i];
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
    public void PlaySE3D(string clip, Vector2 position) 
    {
        if (!_seSources3D[clip].isPlaying)
        {
            _seSources3D[clip].transform.position = position;
            _seSources3D[clip].PlayOneShot(_seSources3D[clip].clip);
        }
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
