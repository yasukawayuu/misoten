using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    protected int _id = 0;
    public int ID
    {
        get {  return _id;  }
        set {  _id = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // âÊñ äOÇ…Ç¢ÇÈèÍçá
        if (!(viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1))
        {
            
        }
    }
}
