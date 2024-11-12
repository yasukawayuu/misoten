using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCamera : MonoBehaviour
{
    [SerializeField] GameObject _target;
    Vector3 _pos;
    // Start is called before the first frame update
    void Start()
    {
        _pos = Camera.main.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = _target.transform.position;

        cameraPos.z = -10;
        Camera.main.gameObject.transform.position = cameraPos;

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,_target.transform.localScale.x + 4.0f, 1.0f);
    }
}
