using System;
using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

public class ServerManager : SingletonMonoBehaviour<ServerManager>
{
    private WebSocket _websocket;
    [SerializeField] private GameObject _player;


    IDictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    int _clientId = 0;

    private Vector2 _position = Vector2.zero;

    public int ClientId
    {
        get { return _clientId; }
    }

    public IDictionary<int, GameObject> Players
    {
        get { return _players; }
    }

    public Vector2 forceInput; // プレイヤーの操作入力 (例: WASDなど)

    private void Start()
    {
        ConnectToServer();
    }

    private async void ConnectToServer()
    {
        _websocket = new WebSocket("ws://localhost:8080");

        _websocket.OnOpen += () => {
            Debug.Log("サーバーに接続した");
        };

        _websocket.OnError += (e) => {
            Debug.Log("サーバーに接続できません " + e);
        };

        _websocket.OnClose += (e) => {
            Debug.Log("サーバーから切断された");
        };

        _websocket.OnMessage += (bytes) =>
        {
            // サーバーからのメッセージを処理
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            HandleServerMessage(message);
        };

        await _websocket.Connect();
    }

    private async void OnDestroy()
    {
        if (_websocket != null)
        {
            await _websocket.Close();
        }
    }

    private void Update()
    {
        if (_websocket == null || _websocket.State != WebSocketState.Open)
            return;


#if !UNITY_WEBGL || UNITY_EDITOR
        _websocket?.DispatchMessageQueue();
#endif
    }


    public async void SendInputToServer(Vector2 velocity)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new InputMessage
            {
                type = "input",
                id = _clientId,
                action = "move",
                force = velocity
            };

            string jsonMessage = JsonUtility.ToJson(message);
            await _websocket.SendText(jsonMessage);
        }
    }

    public async void SendSclaeToServer(int point)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new ScaleMessage
            {
                type = "scale",
                id = _clientId,
                point = point
            };

            string jsonMessage = JsonUtility.ToJson(message);
            await _websocket.SendText(jsonMessage);
        }
    }

    private void HandleServerMessage(string message)
    {
        // サーバーからのメッセージをパース
        var data = JsonUtility.FromJson<ServerMessage>(message);

        switch (data.type)
        {
            case "client":
                _clientId = data.id;
                Debug.Log(_clientId);
                break;
            case "newPlayer":
            case "existingPlayer":
                Vector2 position = new Vector2(data.state[0].position.x, data.state[0].position.y);
                GameObject player = Instantiate(_player, position, Quaternion.Euler(new Vector3(0, 0, 0)));
                player.GetComponent<SpriteRenderer>().material.SetColor("_PlayerColor", data.color);
                if (data.id == _clientId)
                {
                    player.GetComponent<Player>().IsLocalPlayer = true;
                }
                _players[data.id] = player;
                break;
            case "update":
                HandleUpdate(data.state);
                break;
            case "playerDisconnected":
                Debug.Log($"プレイヤーが切断: ID={data.id}");
                Destroy(_players[data.id]);
                _players.Remove(data.id);
                break;
            default:
                Debug.Log($"未処理のメッセージタイプ: {data.type}");
                break;
        }
    }

    private void HandleUpdate(ServerState[] states)
    {
        foreach (var state in states)
        {
            _position = state.position;
            _players[state.id].transform.position = _position;
        }
    }

    [Serializable]
    private class ServerMessage
    {
        public string type;
        public int id;
        public Color color;
        public ServerState[] state;
    }

    [Serializable]
    private class ServerState
    {
        public int id;
        public Vector2 position;
        public Vector2 velocity;
        public float angle;
        public float angularVelocity;
    }

    [Serializable]
    private class InputMessage
    {
        public string type;
        public int id;
        public string action;
        public Vector2 force;
    }

    [Serializable]
    private class ScaleMessage
    {
        public string type;
        public int id;
        public int point;
    }
}
