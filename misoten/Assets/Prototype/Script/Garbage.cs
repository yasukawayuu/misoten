using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation *= Quaternion.Euler(0.0f, 0.0f, 1.0f);
    }
}
