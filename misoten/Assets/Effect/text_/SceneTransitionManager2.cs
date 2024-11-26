using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 确保引用了正确的命名空间

public class SceneTransitionManager2 : MonoBehaviour
{
    public Image circleImage;
    public Image blackBackgroundImage;

    public float fadeDuration = 1f;
    public float maxScale = 10f;

    private void Start()
    {
        circleImage.gameObject.SetActive(true);
        blackBackgroundImage.gameObject.SetActive(true);
        circleImage.rectTransform.localScale = Vector3.zero;

        StartCoroutine(CircleExpandAndMakeTransparent());
    }

    private IEnumerator CircleExpandAndMakeTransparent()
    {
        float timer = 0f;
        Color circleColor = circleImage.color;
        Color blackColor = blackBackgroundImage.color;

        circleImage.color = new Color(circleColor.r, circleColor.g, circleColor.b, 1f);
        blackBackgroundImage.color = new Color(blackColor.r, blackColor.g, blackColor.b, 1f);

        while (timer < fadeDuration)
        {
            float scale = Mathf.Lerp(0f, maxScale, timer / fadeDuration);
            circleImage.rectTransform.localScale = new Vector3(scale, scale, 1f);

            if (scale >= maxScale)
            {
                blackBackgroundImage.color = new Color(blackColor.r, blackColor.g, blackColor.b, 0f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        circleImage.rectTransform.localScale = new Vector3(maxScale, maxScale, 1f);
        circleImage.color = new Color(circleColor.r, circleColor.g, circleColor.b, 0f);
        blackBackgroundImage.gameObject.SetActive(false);
        circleImage.gameObject.SetActive(false);
    }
}
