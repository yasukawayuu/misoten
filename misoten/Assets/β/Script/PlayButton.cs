using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Button _playeButton;

    // Start is called before the first frame update
    void Start()
    {
        _playeButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    private void OnClick()
    {
        //Screen.fullScreen = true;
        //Screen.SetResolution(1920, 1080, true);

        GameSceneManager gameSceneManager = GameSceneManager.Instance;
        gameSceneManager.IsFade = false;
        gameSceneManager.SceneName = "Marimo.io";
        SavePlayerName.Instance.SetName(); 
    }
}
