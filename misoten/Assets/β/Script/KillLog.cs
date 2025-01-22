using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLog : MonoBehaviour
{
    [SerializeField] GameObject _perpetrator;
    [SerializeField] GameObject _victim;
    [SerializeField] GameObject _kill;

    Text _perpetratorText;
    Text _victimText;

    RectTransform _perpetratorRect;
    RectTransform _killRect;
    RectTransform _victimRect;

    Vector2 _sizeDelta = Vector2.zero;

    IEnumerator currentCoroutine = null;    

    public string PerpetratorText
    {
        get { return _perpetratorText.text; }
    }

    public string VictimText
    {
        get { return _victimText.text; }
    }

    private void Start()
    {
        _perpetratorText = _perpetrator.GetComponent<Text>();
        _victimText = _victim.GetComponent<Text>();

        _killRect = _kill.GetComponent<RectTransform>();

        _perpetratorRect = _perpetrator.GetComponent<RectTransform>();
        _victimRect = _victim.GetComponent<RectTransform>();

        _sizeDelta.y = 45;

    }
    public void SetKillLog(string perpetratorText, string victimText)
    {
        _perpetratorText.text = perpetratorText;
        float textWidth = GetTextWidth(_perpetratorText);
        _sizeDelta.x = textWidth;
        _perpetratorRect.sizeDelta = _sizeDelta;

        _killRect.anchoredPosition = new Vector2(textWidth + 20f, 0);
        _victimRect.anchoredPosition = new Vector2(_killRect.anchoredPosition.x + 170f, 0);

        _victimText.text = victimText;
        textWidth = GetTextWidth(_victimText);
        _sizeDelta.x = textWidth;
        _victimRect.sizeDelta = _sizeDelta;

        _perpetratorText.color = Color.black;
        _kill.GetComponent<Image>().color = Color.white;
        _victimText.color = Color.black;
        
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        StartCoroutine(Transparency());
    }

    private IEnumerator Transparency()
    {
        
        yield return new WaitForSeconds(5f);

        float duration = 1.0f;
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;

            _perpetratorText.color = Vector4.Lerp(Color.black, new Vector4(0.0f, 0.0f, 0.0f, 0.0f), t);
            _kill.GetComponent<Image>().color = Vector4.Lerp(Color.white, new Vector4(1.0f, 1.0f, 1.0f, 0.0f), t);
            _victimText.color = Vector4.Lerp(Color.black, new Vector4(0.0f, 0.0f, 0.0f, 0.0f), t);

            yield return null;
        }

        _perpetratorText.color = new Color(0, 0, 0, 0);
        _kill.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _victimText.color = new Color(0, 0, 0, 0);
    }

    public bool IsActive()
    {
        return _perpetratorText.color.a > 0 &&
               _kill.GetComponent<Image>().color.a > 0 &&
               _victimText.color.a > 0;
    }

    private float GetTextWidth(Text text)
    {
        TextGenerationSettings settings = text.GetGenerationSettings(Vector2.zero);
        return text.cachedTextGenerator.GetPreferredWidth(text.text, settings) / text.pixelsPerUnit;
    }
}
