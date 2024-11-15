using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumGarbage : Garbage
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (transform.childCount == 0)
        {
            WebSocketClient client = WebSocketClient.Instance;
            client.GarbageIDSync("mediumGarbage", _id);
            Destroy(gameObject);
        }
    }
}

