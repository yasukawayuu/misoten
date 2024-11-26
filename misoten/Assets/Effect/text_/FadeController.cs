using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // 全画面を覆う黒色のImage
    [SerializeField] private float fadeDuration = 1.0f; // フェードイン・フェードアウトの時間

    private bool isFading = false; // 現在フェード中かどうか
    private bool isFaded = true;  // 現在画面が完全に黒かどうか
    private float fadeTimer = 0.0f; // フェードの経過時間

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeController: fadeImageが設定されていません！");
            return;
        }

        // 初期状態は黒画面（フェードインを開始）
        fadeImage.color = new Color(0, 0, 0, 1);
        StartFadeIn();
    }

    private void Update()
    {
        // スペースキーを押したらフェードインまたはフェードアウトを切り替える
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            if (isFaded)
                StartFadeIn();
            else
                StartFadeOut();
        }
    }

    public void StartFadeIn()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1.0f, 0.0f)); // 黒から透明へフェード
        }
    }

    public void StartFadeOut()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(0.0f, 1.0f)); // 透明から黒へフェード
        }
    }

    private System.Collections.IEnumerator Fade(float startAlpha, float endAlpha)
    {
        isFading = true;
        fadeTimer = 0.0f;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, fadeTimer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, endAlpha);
        isFading = false;

        // フェードインが完了したら、画面が黒くない状態に設定
        if (endAlpha == 0.0f)
        {
            isFaded = false;
        }
        // フェードアウトが完了したら、画面が黒い状態に設定
        else if (endAlpha == 1.0f)
        {
            isFaded = true;
        }
    }
}
