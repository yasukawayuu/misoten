using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections;


public class ServerManager : SingletonMonoBehaviour<ServerManager>
{
    private WebSocket _websocket;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _samllGarbage;
    [SerializeField] private GameObject _mediumGarbage;
    [SerializeField] private GameObject _largeGarbage;

    [SerializeField] private Text _pingText;
    float _ping = 0.0f;
    private Stopwatch _pingStopwatch = new Stopwatch();


    private IDictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
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
        StartCoroutine("SendPing");
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

        //collisionTest();

#if !UNITY_WEBGL || UNITY_EDITOR
        _websocket?.DispatchMessageQueue();
#endif
    }

    private void collisionTest()
    {
        if (Input.GetKey(KeyCode.W))
            SendInputToServer(new Vector2(0.0f, 0.1f));
        if (Input.GetKey(KeyCode.S))
            SendInputToServer(new Vector2(0.0f, -0.1f));
        if (Input.GetKey(KeyCode.A))
            SendInputToServer(new Vector2(-0.1f, 0.0f));
        if (Input.GetKey(KeyCode.D))
            SendInputToServer(new Vector2(0.1f, 0.0f));
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

    public async void SendSclaeToServer(float scale)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new ScaleMessage
            {
                type = "scale",
                id = _clientId,
                scale = scale
            };

            string jsonMessage = JsonUtility.ToJson(message);
            await _websocket.SendText(jsonMessage);
        }
    }

    public async void SendGarbageToServer(string type,int id)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new ScaleMessage
            {
                type = type,
                id = id,
            };

            string jsonMessage = JsonUtility.ToJson(message);
            await _websocket.SendText(jsonMessage);
        }
    }

    private async void SendNameToServer(string name)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new NameMessage
            {
                type = "playerName",
                id = _clientId,
                name = name
            };

            string jsonMessage = JsonUtility.ToJson(message);
            await _websocket.SendText(jsonMessage);
        }
    }

    private async void SendPingToServer()
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new ServerMessage
            {
                type = "ping",
            };

            _pingStopwatch.Restart();
            string jsonMessage = JsonUtility.ToJson(message);
            await _websocket.SendText(jsonMessage);
        }
    }

    private void HandleServerMessage(string message)
    {
        // サーバーからのメッセージをパース
        var data = JsonUtility.FromJson<ServerMessage>(message);
        var garbageData = JsonUtility.FromJson<GarbageMessage>(message);

        switch (data.type)
        {
            case "client":
                _clientId = data.id;
                Debug.Log(_clientId);
                break;
            case "playerName":
                _players[data.id].name = data.name;
                break;
            case "newPlayer":
            case "existingPlayer":
                Debug.Log(data.type);
                Vector2 position = new Vector2(data.state[0].position.x, data.state[0].position.y);
                GameObject player = Instantiate(_player, position, Quaternion.Euler(new Vector3(0, 0, 0)));
                player.GetComponent<SpriteRenderer>().material.SetColor("_PlayerColor", data.color);
                if (data.id == _clientId)
                {
                    player.GetComponent<Player>().IsLocalPlayer = true;
                    player.name = SavePlayerName.Instance.PlayerName;
                    player.GetComponent<Player>().NameText.GetComponent<TextMesh>().text = SavePlayerName.Instance.PlayerName;
                    SendNameToServer(player.name);
                }
                else
                {
                    player.name = data.name;
                    player.GetComponent<Player>().NameText.GetComponent<TextMesh>().text = data.name;
                }   
                _players[data.id] = player;
                break;
            case "smallGarbagePosition":
                GabageSpawn(_samllGarbage, garbageData, "smallGarbage");
                break;
            case "mediumGarbagePosition":
                GabageSpawn(_mediumGarbage, garbageData, "mediumGarbage");
                break;
            case "largeGarbagePosition":
                Instantiate(_largeGarbage, garbageData.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
                break;
            case "update":
                HandleUpdate(data.state);
                break;
            case "pong":
                _pingStopwatch.Stop();
                _ping = _pingStopwatch.ElapsedMilliseconds;
                _pingText.text = _ping.ToString() + "　　ms";
                break;
            case "playerDisconnected":
                Debug.Log($"プレイヤーが切断: ID={data.id}");

                GameObject obj = Instantiate(_player.GetComponent<Player>().DeathParticle, _players[data.id].transform.position, Quaternion.identity);
                var main = obj.GetComponent<ParticleSystem>().main;
                Color color = GetComponent<SpriteRenderer>().material.GetColor("_PlayerColor");
                main.startColor = new Color(color.r, color.g, color.b, 1.0f);

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

    private void GabageSpawn(GameObject gabage, GarbageMessage data, string type)
    {
        if (IsPlayerNearby(new Vector2(data.position.x, data.position.y)))
        {
            GameObject garbage = Instantiate(gabage, data.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
            garbage.GetComponent<Garbage>().ID = data.id;
        }
        else
        {
            SendGarbageToServer(type, data.id);
        }
    }

    private bool IsPlayerNearby(Vector2 position)
    {
        float radius = 5.0f;  // 衝突判定の範囲（半径）
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                return false;  // プレイヤーが近くにいる
            }
        }

        return true;  // プレイヤーがいない

    }

    private IEnumerator SendPing()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            SendPingToServer();
        }
    }

    public async void CloseWebSocket()
    {
        if (_websocket != null && _websocket.State == WebSocketState.Open)
        {
            await _websocket.Close();
            Debug.Log("サーバーから切断しました");
        }
    }

    [Serializable]
    private class ServerMessage
    {
        public string type;
        public int id;
        public string name;
        public Color color;
        public ServerState[] state;
    }

    [Serializable]
    private class ServerState
    {
        public int id;
        public Vector2 position;
        public long timestamp;
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
        public float scale;
    }

    [Serializable]
    private class GarbageMessage
    {
        public string type;
        public int id;
        public Vector2 position;
    }

    [Serializable]
    private class NameMessage
    {
        public string type;
        public int id;
        public string name;
    }
}
