using System;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;    
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;


public class ServerManager : SingletonMonoBehaviour<ServerManager>
{
    private WebSocket _websocket;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _samllGarbage;
    [SerializeField] private GameObject _mediumGarbage;
    [SerializeField] private GameObject _largeGarbage;
    
    private int _stateID;

    [SerializeField] private Text _pingText;
    float _ping = 0.0f;
    private Stopwatch _pingStopwatch = new Stopwatch();


    private IDictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    int _clientId = 0;

    private Dictionary<int, Vector3> _targetPositions = new Dictionary<int, Vector3>();
    private Dictionary<int, Vector3> _currentVelocities = new Dictionary<int, Vector3>();

    private float _lerpSpeed = 10f; // 補間速度
    private float _latencyCompensation = 0.1f; // 遅延補正時間（秒）

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
        StartCoroutine("SendPoint");
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

        foreach (var playerEntry in _players)
        {
            int id = playerEntry.Key;
            GameObject player = playerEntry.Value;

            if (_targetPositions.ContainsKey(id))
            {
                // 現在の位置
                Vector3 currentPosition = player.transform.position;

                // 目標位置
                Vector3 targetPosition = _targetPositions[id];

                // 距離に応じた補間速度を動的に計算
                float distance = Vector3.Distance(currentPosition, targetPosition);
                float dynamicLerpSpeed = Mathf.Clamp(distance * _lerpSpeed, _lerpSpeed, _lerpSpeed * 2f);

                // スムーズな補間
                Vector3 smoothedPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * dynamicLerpSpeed);

                //ターゲットに方向を向かせる
                Vector2 direction = targetPosition - currentPosition;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
                GameObject capsule = player.transform.GetChild(player.transform.childCount - 1).gameObject;

                //カプセルの長さ調整をする
                capsule.transform.rotation = rotation;
                CapsuleCollider2D capsuleCollider = capsule.GetComponent<CapsuleCollider2D>();
                float size = Mathf.Round(distance * 10f) / 10f;
                capsuleCollider.size = new Vector2(0.25f, size + 0.25f);
                if (capsuleCollider.size.y > 0.25f)
                    capsuleCollider.offset = new Vector2(0.0f, -capsuleCollider.size.y / 2);
                else
                    capsuleCollider.offset = new Vector2(0.0f, 0.0f);

                // オブジェクトを移動させる
                player.transform.position = smoothedPosition;
            }
        }

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

    private async void SendPointToServer(float point)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            // 入力データをサーバーに送信
            var message = new PointMessage
            {
                type = "point",
                id = _clientId,
                point = point
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
        var pointData = JsonUtility.FromJson<PointMessage>(message);

        switch (data.type)
        {
            case "client":
                _clientId = data.id;
                Debug.Log(_clientId);
                break;
            case "playerName":
                _players[data.id].name = data.name;
                break;
            case "point":
                _players[data.id].GetComponent<Player>().Point = pointData.point;
                break;
            case "newPlayer":
            case "existingPlayer":
                Debug.Log(data.type);
                Vector3 position = new Vector3(data.state[0].position.x, data.state[0].position.y,0);
                GameObject player = Instantiate(_player, position, Quaternion.Euler(new Vector3(0, 0, 0)));
                player.GetComponent<SpriteRenderer>().material.SetColor("_PlayerColor", data.color);
                if (data.id == _clientId)
                {
                    player.GetComponent<Player>().IsLocalPlayer = true;
                    player.GetComponent<Player>().MapRender.color = Color.red;
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
                Color color = _players[data.id].GetComponent<SpriteRenderer>().material.GetColor("_PlayerColor");
                main.startColor = new Color(color.r, color.g, color.b, 1.0f);

                StartCoroutine(_players[data.id].GetComponent<Player>().Disolv());

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
            if (!_players.ContainsKey(state.id))
            {
                continue;
            }

            // 遅延を考慮してターゲット位置を計算
            Vector3 predictedPosition = state.position + state.velocity * _latencyCompensation;

            // ターゲット位置と速度を保存
            _targetPositions[state.id] = predictedPosition;
            _currentVelocities[state.id] = state.velocity; // サーバーからの速度も記録
        }
    }

    private void GabageSpawn(GameObject gabage, GarbageMessage data, string type)
    {
        if (IsPlayerNearby(new Vector2(data.position.x, data.position.y)))
        {
            GameObject garbage = Instantiate(gabage,new Vector3(data.position.x,data.position.y,-10.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
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

    private IEnumerator SendPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SendPointToServer(Players[_clientId].GetComponent<Player>().Point);
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
        public Vector2 velocity;
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

    [Serializable]
    private class PointMessage
    {
        public string type;
        public int id;
        public float point;
    }
}
