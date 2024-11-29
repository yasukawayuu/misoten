using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Button _playeButton;
    [SerializeField] ChangeScene ChangeScene;
    GameObject _playerName;

    // Start is called before the first frame update
    void Start()
    {
        _playerName = GameObject.Find("PlayerName");
        _playeButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    private void OnClick()
    {
        ChangeScene.LoadScene("Marimo.io");
        _playerName.GetComponent<SavePlayerName>().SetName();
    }

}
