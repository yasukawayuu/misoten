using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRenderTextureResize : MonoBehaviour
{
    [SerializeField] private Camera _effectCamera;
    [SerializeField] private RenderTexture _renderTexture;

    private Vector2Int _previousScreenSize;

    void Awake()
    {
        _previousScreenSize = new Vector2Int(Screen.width, Screen.height);
        OnScreenSizeChanged(_previousScreenSize);
    }

    void Start()
    {
        if (_effectCamera == null)
        {
            _effectCamera = Camera.main;
        }

        float height = _effectCamera.orthographicSize * 2;
        float width = height * _effectCamera.aspect;

        transform.localScale = new Vector3(width, height, 0);
    }

    void Update()
    {
        Vector2Int currentScreenSize = new Vector2Int(Screen.width, Screen.height);
        if (currentScreenSize != _previousScreenSize)
        {
            OnScreenSizeChanged(currentScreenSize);
        }
    }

    void OnScreenSizeChanged(Vector2Int newSize)
    {
        _renderTexture = new RenderTexture(newSize.x, newSize.y, 16);
    }
}
