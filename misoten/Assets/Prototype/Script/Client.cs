using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System.Threading.Tasks;
using System.Text;

public class Client : MonoBehaviour
{
    WebSocket websocket;

    async void Start()
    {
        // サーバーに接続
        websocket = new WebSocket("ws://localhost:443");

        //サーバーに接続成功した時
        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        //サーバーに接続失敗した時
        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        //サーバーに接続閉じたとき
        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        // サーバーからメッセージを受信
        websocket.OnMessage += (bytes) =>
        {
            // バイトデータを文字列に変換
            var message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Message received from server: " + message);
        };

        // 接続を確立
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        // スペースキーが押されたときに「Hello Unity」を送信
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageToServer("Hello Unity");
        }
    }

    async void OnApplicationQuit()
    {
        // WebSocket切断
        await websocket.Close();
    }

    // メッセージをサーバーに送信
    public async void SendMessageToServer(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(message);
        }
    }
}
