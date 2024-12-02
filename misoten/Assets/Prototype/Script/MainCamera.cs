using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCamera : MonoBehaviour
{
    Vector3 _pos;
    [SerializeField] Camera _effectCamera;
    // Start is called before the first frame update
    void Start()
    {
        _pos = Camera.main.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        WebSocketClient client = WebSocketClient.Instance;
        if (client.Players.ContainsKey(client.ClientId))
        {
            Vector3 cameraPos = client.Players[client.ClientId].transform.position;


            cameraPos.z = -10;
            Camera.main.gameObject.transform.position = cameraPos;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, client.Players[client.ClientId].transform.localScale.x + 4.0f, 1.0f);
            _effectCamera.orthographicSize = Camera.main.orthographicSize;
        }
    }

}
