using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRenderTextureResize : MonoBehaviour
{
    [SerializeField] private Camera effectCamera;

    void Start()
    {
        if (effectCamera == null)
        {
            effectCamera = Camera.main;
        }

        float height = effectCamera.orthographicSize * 2;
        float width = height * effectCamera.aspect;

        transform.localScale = new Vector3(width, height, 0);
    }
}
