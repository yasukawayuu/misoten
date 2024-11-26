using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    // フェードインとフェードアウト用のキャンバス
    public Image fadeImage;
    public float fadeDuration = 1f; // フェードイン/フェードアウトの時間

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // シーンがロードされた際にイベントをリスンする
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 初期状態でフェードインを開始
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());

    }

    // シーンがロードされた後に呼ばれる
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage == null)
        {
            return;
        }
        // フェードインが完了した後にフェードアウトを開始
        StartCoroutine(FadeOut());
    }

    // フェードインの処理
    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        // 黒から透明に徐々にフェードイン
        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        // 最後に完全に透明に設定
        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }

    // フェードアウトの処理
    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        Color color = fadeImage.color;

        // 透明から黒に徐々にフェードアウト
        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        // 最後に完全に黒に設定
        color.a = 1f;
        fadeImage.color = color;
    }
}
