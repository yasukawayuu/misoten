using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePlayerName : SingletonMonoBehaviour<SavePlayerName>
{
    [SerializeField]private string _playerName = "";
    private InputField _inputField;

    public string PlayerName
    {
        get { return _playerName; } 
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void SetName()
    {
        _inputField = GameObject.Find("NickName").GetComponent<InputField>();
        _playerName = _inputField.text;
    }
}
