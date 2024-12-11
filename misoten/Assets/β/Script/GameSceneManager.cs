using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class GameSceneManager : SingletonMonoBehaviour<GameSceneManager>
{
    [SerializeField] private Material circleFadeMaterial; // 円形フェードのマテリアル
    [SerializeField] private GameObject _panel;
    private float maskProgress = 0f; // マスクの現在の進行度
    private bool _isFade = true; // マスクが拡大中か、trueで拡大、falseで縮小
    private string _sceneName = string.Empty;

    [SerializeField] private float speed = 0.5f; // マスクの拡大/縮小速度

    public bool IsFade
    {
        set { _isFade = value; }
    }

    public string SceneName
    {
        set { _sceneName = value; }
    }
    void Start()
    {
        // マテリアルの _MaskProgress プロパティを 0 に初期化
        if (circleFadeMaterial != null)
        {
            circleFadeMaterial.SetFloat("_MaskProgress", maskProgress);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        // スペースキーを押した時に拡大/縮小状態を切り替える
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    isFade = !isFade;
        //}

        // 状態に応じてマスクの進行度を更新
        if (_isFade)
        {
            maskProgress += speed * Time.deltaTime; // 拡大
            if (maskProgress > 1f)
            {
                _panel.SetActive(false);
                maskProgress = 1f;
            }// 最大値を1に制限
        }
        else
        {
            _panel.SetActive(true);
            maskProgress -= speed * Time.deltaTime; // 縮小
            if (maskProgress < 0f)
            {
                SceneManager.LoadScene(_sceneName);
                maskProgress = 0f;
                _isFade = true;
            }// 最小値を0に制限
        }

        // マテリアルの _MaskProgress プロパティを更新
        if (circleFadeMaterial != null)
        {
            circleFadeMaterial.SetFloat("_MaskProgress", maskProgress);
        }
    }
}
