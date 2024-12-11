using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRenderTextureResize : MonoBehaviour
{
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private Camera _effectCamera;
    private Camera _mainCamera;
    private Vector2Int _previousScreenSize;

    void Start()
    {
        _mainCamera = Camera.main;

        int width = (int)_mainCamera.pixelRect.width;
        int height = (int)_mainCamera.pixelRect.height;
        _previousScreenSize = new Vector2Int(width, height);
        OnScreenSizeChanged(_previousScreenSize);
    }

    void Update()
    {
        int width = (int)_mainCamera.pixelRect.width;
        int height = (int)_mainCamera.pixelRect.height;
        Vector2Int currentScreenSize = new Vector2Int(width, height);
        if (currentScreenSize != _previousScreenSize)
        {
            OnScreenSizeChanged(currentScreenSize);
            _previousScreenSize = currentScreenSize;
        }
    }

    void OnScreenSizeChanged(Vector2Int newSize)
    {
        // カメラ更新
        _effectCamera.rect = _mainCamera.rect;
        _effectCamera.aspect = _mainCamera.aspect;

        // renderTextureのsizeを更新
        _renderTexture.Release();
        _renderTexture.width = newSize.x;
        _renderTexture.height = newSize.y;
        _renderTexture.Create();

        float height = _mainCamera.orthographicSize * 2;
        float width = height * _mainCamera.aspect;

        transform.localScale = new Vector3(width, height, 0);
    }
}