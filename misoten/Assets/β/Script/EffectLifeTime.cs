using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kill");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Ž€–S
    /// </summary>
    /// <returns></returns>
    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
