using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawn : MonoBehaviour
{
    // 回転する範囲（最大角度）
    public float rotationAngle = 15f;

    // 回転の速さ（周期の調整）
    public float rotationSpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
