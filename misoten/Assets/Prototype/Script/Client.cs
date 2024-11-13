using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections;

public class WebSocketClient : SingletonMonoBehaviour<WebSocketClient>
{
    WebSocket websocket;
    [SerializeField] Text text;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _samllGarbage;
    [SerializeField] private GameObject _mediumGarbage;
    [SerializeField] private GameObject _LargeGarbage;

    IDictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    int _clientId = 0;

    private float ping;
    private Stopwatch pingStopwatch = new Stopwatch();

    public int ClientId
    {
        get { return _clientId; }
    }
    public IDictionary<int, GameObject> Players
    {
        get { return _players; }
    }

    void Start()
    {
        ConnectWebSocket();
        StartCoroutine("PingSync");
    }

    private async void ConnectWebSocket()
    {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () => {
            Debug.Log("サーバーに接続した");
        };

        websocket.OnError += (e) => {
            Debug.Log("サーバーに接続できません " + e);
        };

        websocket.OnClose += (e) => {
            Debug.Log("サーバーから切断された");
        };

        websocket.OnMessage += (bytes) => {
            var message = Encoding.UTF8.GetString(bytes);

            // JSON形式のメッセージを解析
            var data = JsonUtility.FromJson<ServerMessage>(message);


            if(data.type == "client")
            {
                _clientId = data.id;
                Debug.Log(_clientId);
            }
            else if (data.type == "existingPlayer" || data.type == "newPlayer")
            {
                GameObject player = Instantiate(_player, data.position, Quaternion.Euler(data.rotation));
                player.transform.localScale = data.scale;
                if(data.id == _clientId)
                    player.GetComponent<Player>().IsLocalPlayer = true;
                _players[data.id] = player;

                Debug.Log($"新しいプレイヤーが接続: {data.id}");
            }
            else if (data.type == "transform")
            {
                _players[data.id].transform.position = data.position;
                _players[data.id].transform.rotation = Quaternion.Euler(data.rotation);
                _players[data.id].transform.localScale = data.scale;
                _players[data.id].GetComponent<Rigidbody2D>().velocity = data.velocity;
                _players[data.id].GetComponent<Rigidbody2D>().angularVelocity  = data.angularVelocity;
            }
            else if(data.type == "samllGarbagePosition")
            {
                GabageSpawn(_samllGarbage, data, "smallGarbage");
            }
            else if (data.type == "mediumGarbagePosition")
            {
                GabageSpawn(_mediumGarbage, data, "mediumGarbage");
            }
            else if (data.type == "largeGarbagePosition")
            {
                Instantiate(_LargeGarbage, data.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
            }
            else if (data.type == "pong")
            {
                pingStopwatch.Stop();
                ping = pingStopwatch.ElapsedMilliseconds;
                text.text = ping.ToString() + "　　ms";
            }
            else if (data.type == "playerDisconnected")
            {
                Debug.Log($"{data.id} が切断しました");
                Destroy(_players[data.id]);
                _players.Remove(data.id);
            }
        };

        await websocket.Connect();
    }

    private void Update()
    {
        if (_players.ContainsKey(_clientId))
        {
            PlayerSync(_players[_clientId]); // プレイヤーの位置情報をサーバーに送信
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    private void FixedUpdate()
    {
        
    }

    public async void GarbageIDSync(string gabageType,int _mapchipID)
    {
        if (websocket.State == WebSocketState.Open)
        {
            var data = new ServerMessage
            {
                type = gabageType,
                id = _mapchipID
            };

            await websocket.SendText(JsonUtility.ToJson(data));
        }
    }

    private async void PlayerSync(GameObject player)
    {
        if (websocket.State == WebSocketState.Open)
        {
            var data = new ServerMessage
            {
                type = "transform",
                id = _clientId,
                position = player.transform.position,
                rotation = player.transform.rotation.eulerAngles,
                scale = player.transform.localScale,
                velocity = player.GetComponent<Rigidbody2D>().velocity,
                angularVelocity = player.GetComponent<Rigidbody2D>().angularVelocity

            };

            await websocket.SendText(JsonUtility.ToJson(data));
        }
    }

    async void SendPing()
    {
        if (websocket.State == WebSocketState.Open)
        {
            var data = new ServerMessage
            {
                type = "ping",
            };
            pingStopwatch.Restart();
            await websocket.SendText(JsonUtility.ToJson(data));
        }
    }

    private bool IsPlayerNearby(Vector3 position)
    {
        // プレイヤーの位置と半径を基に周囲のプレイヤーをチェック
        float radius = 5.0f;  // 衝突判定の範囲（半径）
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                return false;  // プレイヤーが近くにいる
            }
        }

        return true;  // プレイヤーがいない
    }

    private void GabageSpawn(GameObject gabage, ServerMessage data, string garbageType)
    {
        if (IsPlayerNearby(data.position))
        {
            GameObject garbage = Instantiate(gabage, data.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
            garbage.GetComponent<Garbage>().ID = data.id;
        }
        else
        {
            GarbageIDSync(garbageType, data.id);
        }
    }

    private IEnumerator PingSync()
    {
        yield return new WaitForSeconds(2f);
        SendPing();
    }   
    async void OnApplicationQuit()
    {
        await websocket.Close();
    }


    // サーバーから受け取るメッセージのデータ形式
    [Serializable]
    public class ServerMessage
    {
        public string type; // メッセージの種類 ("id" or "message")
        public int id; // クライアントID
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Vector2 velocity;
        public float angularVelocity;
    }
}
