using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket websocket;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket接続が確立されました");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("受信メッセージ: " + message);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("エラー: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("WebSocket接続が閉じられました: " + e);
        };

        await websocket.Connect();
    }


    private async void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
                websocket.DispatchMessageQueue();
        #endif

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("スペースキーが押されました。メッセージを送信します。");
            if (websocket.State == WebSocketState.Open)
                await websocket.SendText("Unityからのメッセージ");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
