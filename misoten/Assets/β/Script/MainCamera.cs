using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCamera : MonoBehaviour
{
    Vector3 _pos;
    [SerializeField] private Camera _effectCamera;

    // Start is called before the first frame update
    void Start()
    {
        _pos = Camera.main.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ServerManager client = ServerManager.Instance;
        if (client.Players.ContainsKey(client.ClientId))
        {

            Vector3 targetCameraPosition = client.Players[client.ClientId].transform.position;
            Vector3 currentCameraPosition = Camera.main.gameObject.transform.position;
            targetCameraPosition.z = -10;
            Camera.main.gameObject.transform.position = Vector3.Lerp(currentCameraPosition, targetCameraPosition,0.125f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, client.Players[client.ClientId].transform.localScale.x + 4.0f, 1.0f);
        }

        _effectCamera.orthographicSize = Camera.main.orthographicSize;
    }

}
