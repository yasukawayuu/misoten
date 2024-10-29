using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour
{
    [SerializeField] GameObject _garbage;

    void Start()
    {
        StartCoroutine("Spawn");
    }
    private IEnumerator Spawn()
   {
        while (true)
        {
            Instantiate(_garbage, new Vector3(UnityEngine.Random.Range(-100.0f, 100.0f), UnityEngine.Random.Range(-100.0f, 100.0f), 0.0f), Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
   }
}
