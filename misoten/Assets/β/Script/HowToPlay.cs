using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _images = null;
    private int _htpCount = 0;

    void Start()
    {
        if(_images.Length != 0)
            _image.sprite = _images[_htpCount];
    }
    
    public void SetImage(int count)
    {
        if (_images.Length != 0 &&
            _htpCount + count >= 0 && _htpCount + count < _images.Length)
        {
            _htpCount += count;
            _image.sprite = _images[_htpCount];
        }
    }

}
