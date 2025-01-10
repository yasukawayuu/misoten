using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image[] _images = null;
    private int _htpCount = 0;

    void Start()
    {
        if(_images.Length != 0)
            _image = _images[_htpCount];
    }
    
    public void SetImage(int count)
    {
        if (_images.Length != 0 && _htpCount >= 0 && _htpCount <= _images.Length)
        {
            _htpCount += count;
            _image = _images[_htpCount];
        }
    }

}
