using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;

public class PlayerManager : MonoBehaviour
{
    private WebSocket websocket;

    // UI要素への参照
    public InputField usernameInput;
    public InputField messageInput;
    public Button sendButton;
    public Text chatText;
    public InputField changeNameInput;
    public Button changeNameButton;

    // プレイヤーの情報
    private int yourID = -1;
    private string username = "Anonymous";

    // 他のプレイヤー情報を格納
    private Dictionary<int, PlayerTestManager> otherPlayers = new Dictionary<int, PlayerTestManager>();

    // プレイヤーのPrefab（事前に作成しておく）
    public GameObject playerPrefab;

    // 自分のプレイヤーオブジェクト
    private PlayerTestManager localPlayer;

    void Start()
    {
        // WebSocketの接続先を設定
        websocket = new WebSocket("ws://13.114.252.177:8080/"); // サーバーのIPアドレスとポートに変更してください

        // イベントハンドラの設定
        websocket.OnOpen += OnWebSocketOpen;
        websocket.OnError += OnWebSocketError;
        websocket.OnClose += OnWebSocketClose;
        websocket.OnMessage += OnWebSocketMessage;

        // Sendボタンが押されたときのイベントハンドラを設定
        sendButton.onClick.AddListener(SendChatMessage);

        // ChangeNameボタンが押されたときのイベントハンドラを設定
        changeNameButton.onClick.AddListener(ChangePlayerName);

        // WebSocket接続を開始
        websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        HandleMovement();
    }

    private void OnApplicationQuit()
    {
        websocket.Close();
    }

    // WebSocket接続が開いたときの処理
    void OnWebSocketOpen()
    {
        Debug.Log("WebSocket接続が開かれました");
        AppendChatMessage("システム", "チャットサーバーに接続しました");
    }

    // WebSocketエラー時の処理
    void OnWebSocketError(string error)
    {
        Debug.Log("WebSocketエラー: " + error);
        AppendChatMessage("システム", $"エラー: {error}");
    }

    // WebSocket接続が閉じたときの処理
    void OnWebSocketClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("WebSocket接続が閉じられました");
        AppendChatMessage("システム", "チャットサーバーとの接続が切れました");
    }

    // WebSocketでメッセージを受信したときの処理
    void OnWebSocketMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("受信メッセージ: " + message);

        try
        {
            var json = JsonUtility.FromJson<ServerMessage>(message);
            switch (json.type)
            {
                case "init":
                    HandleInit(json);
                    break;
                case "existing_clients":
                    HandleExistingClients(json);
                    break;
                case "new_client":
                    HandleNewClient(json);
                    break;
                case "update_move":
                    HandleUpdateMove(json);
                    break;
                case "update_name":
                    HandleUpdateName(json);
                    break;
                case "new_chat":
                    HandleNewChat(json);
                    break;
                case "client_disconnect":
                    HandleClientDisconnect(json);
                    break;
                default:
                    Debug.LogWarning("未知のメッセージタイプ: " + json.type);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("メッセージの解析に失敗しました: " + e);
        }
    }

    // メッセージタイプごとのハンドラ
    void HandleInit(ServerMessage msg)
    {
        yourID = msg.yourID;
        Debug.Log($"あなたのIDは {yourID} です");

        // 自分のプレイヤーを生成
        GameObject playerObj = Instantiate(playerPrefab, new Vector3(msg.x, msg.y, 0), Quaternion.identity);
        localPlayer = playerObj.GetComponent<PlayerTestManager>();
        localPlayer.SetID(yourID);
        localPlayer.SetName(username); // 初期名前はローカルで設定されたもの

        // 自分のプレイヤーを特定のレイヤーに設定するなどの処理が必要な場合はここで行う
    }

    void HandleExistingClients(ServerMessage msg)
    {
        if (msg.clients == null) return;

        foreach (var client in msg.clients)
        {
            if (!otherPlayers.ContainsKey(client.id) && client.id != yourID)
            {
                AddNewPlayer(client.id, client.name, client.x, client.y);
            }
        }
    }

    void HandleNewClient(ServerMessage msg)
    {
        var client = msg.client;
        if (!otherPlayers.ContainsKey(client.id) && client.id != yourID)
        {
            AddNewPlayer(client.id, client.name, client.x, client.y);
        }
    }

    void HandleUpdateMove(ServerMessage msg)
    {
        if (otherPlayers.ContainsKey(msg.id))
        {
            otherPlayers[msg.id].UpdatePosition(new Vector2(msg.x, msg.y));
        }
    }

    void HandleUpdateName(ServerMessage msg)
    {
        if (otherPlayers.ContainsKey(msg.id))
        {
            otherPlayers[msg.id].UpdateName(msg.name);
        }
    }

    void HandleNewChat(ServerMessage msg)
    {
        string senderName = msg.name;
        string chatMessage = msg.message;
        AppendChatMessage(senderName, chatMessage);
    }

    void HandleClientDisconnect(ServerMessage msg)
    {
        if (otherPlayers.ContainsKey(msg.id))
        {
            Destroy(otherPlayers[msg.id].gameObject);
            otherPlayers.Remove(msg.id);
            AppendChatMessage("システム", $"ユーザー「{msg.name}」が切断しました");
        }
    }

    // プレイヤーの追加
    void AddNewPlayer(int id, string name, float x, float y)
    {
        GameObject playerObj = Instantiate(playerPrefab, new Vector3(x, y, 0), Quaternion.identity);
        PlayerTestManager player = playerObj.GetComponent<PlayerTestManager>();
        player.SetID(id);
        player.SetName(name);
        otherPlayers.Add(id, player);
        AppendChatMessage("システム", $"ユーザー「{name}」が参加しました");
    }

    // プレイヤーの移動入力を処理
    void HandleMovement()
    {
        if (localPlayer == null) return; // 初期化待ち

        float xs = 0.0f;
        float ys = 0.0f;
        float speed = 0.05f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xs = -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            xs = speed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ys = speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            ys = -speed;
        }

        if (xs != 0 || ys != 0)
        {
            // ローカルプレイヤーの位置を更新
            localPlayer.Move(new Vector2(xs, ys));

            // 移動メッセージを送信
            MoveMessage moveMsg = new MoveMessage
            {
                type = "move",
                x = xs,
                y = ys
            };
            string json = JsonUtility.ToJson(moveMsg);
            websocket.SendText(json);
        }
    }

    // チャットメッセージを送信
    async void SendChatMessage()
    {
        string msg = messageInput.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;

        ChatMessage chatMsg = new ChatMessage
        {
            type = "chat",
            message = msg
        };
        string json = JsonUtility.ToJson(chatMsg);
        await websocket.SendText(json);

        // 自分のメッセージをチャットに表示
        //AppendChatMessage(username, msg);

        // メッセージ入力フィールドをクリア
        messageInput.text = "";
    }

    // プレイヤー名を変更
    async void ChangePlayerName()
    {
        string newName = changeNameInput.text.Trim();
        if (string.IsNullOrEmpty(newName)) return;

        username = newName;

        // 名前変更メッセージを送信
        SetNameMessage nameMsg = new SetNameMessage
        {
            type = "set_name",
            name = newName
        };
        string json = JsonUtility.ToJson(nameMsg);
        await websocket.SendText(json);

        // 自分のプレイヤー名を更新
        if (localPlayer != null)
        {
            localPlayer.SetName(newName);
        }

        // 名前入力フィールドをクリア
        changeNameInput.text = "";
    }

    // チャットメッセージをUIに追加
    void AppendChatMessage(string user, string message)
    {
        chatText.text += $"<b>{user}:</b> {message}\n";
    }

    // サーバーメッセージのデータ構造
    [Serializable]
    public class ServerMessage
    {
        public string type;
        public int yourID;

        // existing_clients
        public ExistingClient[] clients;

        // new_client
        public ClientData client;

        // update_move, update_name, new_chat, client_disconnect
        public int id;
        public string name;
        public float x;
        public float y;
        public string message;
    }

    [Serializable]
    public class ExistingClient
    {
        public int id;
        public string name;
        public float x;
        public float y;
    }

    [Serializable]
    public class ClientData
    {
        public int id;
        public string name;
        public float x;
        public float y;
    }

    [Serializable]
    public class MoveMessage
    {
        public string type;
        public float x;
        public float y;
    }

    [Serializable]
    public class SetNameMessage
    {
        public string type;
        public string name;
    }

    [Serializable]
    public class ChatMessage
    {
        public string type;
        public string message;
    }
}
