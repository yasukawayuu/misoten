using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;

public class NativeWS : MonoBehaviour
{
    WebSocket websocket;

    // UI要素への参照
    public InputField usernameInput;
    public InputField messageInput;
    public Button sendButton;
    public Text chatText;

    // ユーザー名
    private string username = "Anonymous";

    // Start is called before the first frame update
    async void Start()
    {
        // WebSocketの接続先を設定
        websocket = new WebSocket("ws://13.114.252.177:8080/");

        // イベントハンドラの設定
        websocket.OnOpen += OnWebSocketOpen;
        websocket.OnError += OnWebSocketError;
        websocket.OnClose += OnWebSocketClose;
        websocket.OnMessage += OnWebSocketMessage;

        // Sendボタンが押されたときのイベントハンドラを設定
        sendButton.onClick.AddListener(SendChatMessage);

        // WebSocket接続を開始
        await websocket.Connect();
    }

    // WebSocket接続が開いたときの処理
    void OnWebSocketOpen()
    {
        Debug.Log("Connection open!");
        AppendChatMessage("システム", "チャットサーバーに接続しました");
    }

    // WebSocketエラー時の処理
    void OnWebSocketError(string error)
    {
        Debug.Log("Error! " + error);
        AppendChatMessage("システム", $"エラー: {error}");
    }

    // WebSocket接続が閉じたときの処理
    void OnWebSocketClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("Connection closed!");
        AppendChatMessage("システム", "チャットサーバーとの接続が切れました");
    }

    // WebSocketでメッセージを受信したときの処理
    void OnWebSocketMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("OnMessage! " + message);

        try
        {
            var data = JsonUtility.FromJson<ChatMessage>(message);
            if (data.system)
            {
                AppendChatMessage("システム", data.message);
            }
            else
            {
                AppendChatMessage(data.username, data.message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("メッセージの解析に失敗しました: " + e);
        }
    }

    // メッセージ送信ボタンが押されたときの処理
    async void SendChatMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            string msg = messageInput.text.Trim();
            string user = usernameInput.text.Trim();

            if (string.IsNullOrEmpty(user))
            {
                user = "Anonymous";
            }

            if (!string.IsNullOrEmpty(msg))
            {
                // メッセージをJSON形式に変換
                ChatMessage chatMessage = new ChatMessage
                {
                    username = user,
                    message = msg
                };

                string json = JsonUtility.ToJson(chatMessage);
                await websocket.SendText(json);

                // 自分のメッセージをチャットに表示
                //AppendChatMessage(user, msg);

                // メッセージ入力フィールドをクリア
                messageInput.text = "";
            }
        }
    }

    // チャットメッセージをUIに追加するメソッド
    void AppendChatMessage(string user, string message)
    {
        chatText.text += $"<b>{user}:</b> {message}\n";
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    // チャットメッセージのデータ構造
    [Serializable]
    public class ChatMessage
    {
        public bool system = false; // システムメッセージかどうか
        public string username;
        public string message;
    }
}
