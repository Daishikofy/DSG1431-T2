using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 randomIntensity;
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition += offset;
        transform.localPosition += new Vector3 (Random.Range(-randomIntensity.x, randomIntensity.x)
                                               ,Random.Range(-randomIntensity.y, randomIntensity.y)
                                               ,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
