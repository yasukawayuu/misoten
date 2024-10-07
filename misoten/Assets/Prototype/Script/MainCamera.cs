using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class MainCamera : MonoBehaviour
{
    [SerializeField]GameObject _target;
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

    }

}
