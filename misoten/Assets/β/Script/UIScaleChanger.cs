using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleChanger : MonoBehaviour
{
    [SerializeField] private Canvas _rankingCanvas;
    [SerializeField] private Canvas _minimapCanvas;
    [SerializeField] private RectTransform _ranking;
    [SerializeField] private RectTransform _miniMap;
    private Rect _rectOne;
    private Rect _rectTwo;
    [SerializeField] private bool _overlapping;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckForOverlap();

        int i = 0;
        while(_overlapping)
        {
            CheckForOverlap();
            _rankingCanvas.scaleFactor = 1.0f - 0.02f * i;
            _minimapCanvas.scaleFactor = 1.0f - 0.02f * i;
            i++;
        }
    }

    public void CheckForOverlap()
    {
        _rectOne = GetWorldRect(_miniMap);
        _rectTwo = GetWorldRect(_ranking);

        if (_rectOne.Overlaps(_rectTwo))
        {
            _overlapping = true;
        }
        else
        {
            _overlapping = false;
        }
    }

    public Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        Vector2 size = new Vector2(rt.rect.size.x, rt.rect.size.y);
        return new Rect(topLeft, size);
    }
}
