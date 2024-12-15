using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class GameSceneManager : SingletonMonoBehaviour<GameSceneManager>
{
    [SerializeField] private Material _circleFadeMaterial; // 円形フェードのマテリアル
    [SerializeField] private GameObject _panel;
    private float _maskProgress = 0f; // マスクの現在の進行度
    private bool _isFade = true; // マスクが拡大中か、trueで拡大、falseで縮小
    private string _sceneName = string.Empty;

    [SerializeField] private float _speed = 0.5f; // マスクの拡大/縮小速度

    public bool IsFade
    {
        get { return _isFade; }
        set { _isFade = value; }
    }

    public string SceneName
    {
        set { _sceneName = value; }
    }

    public float MaskProgress
    {
        get { return _maskProgress; }
    }

    public GameObject Panel
    {
        get { return _panel; }
    }

    void Start()
    {
        // マテリアルの _MaskProgress プロパティを 0 に初期化
        if (_circleFadeMaterial != null)
        {
            _circleFadeMaterial.SetFloat("_MaskProgress", _maskProgress);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        // 状態に応じてマスクの進行度を更新
        if (_isFade)
        {
            _maskProgress += _speed * Time.deltaTime; // 拡大
            if (_maskProgress > 1f)
            {
                _panel.SetActive(false);
                _maskProgress = 1f;
            }// 最大値を1に制限
        }
        else
        {
            _panel.SetActive(true);
            _maskProgress -= _speed * Time.deltaTime; // 縮小
            if (_maskProgress < 0f)
            {
                SceneManager.LoadScene(_sceneName);
                _maskProgress = 0f;
                _isFade = true;
            }// 最小値を0に制限
        }

        // マテリアルの _MaskProgress プロパティを更新
        if (_circleFadeMaterial != null)
        {
            _circleFadeMaterial.SetFloat("_MaskProgress", _maskProgress);
        }
    }
   

}
