using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 randomIntensity;
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition += offset;
        /*transform.localPosition += new Vector3 (Random.Range(-randomIntensity.x, randomIntensity.x)
                                               ,Random.Range(-randomIntensity.y, randomIntensity.y)
                                               ,0*/
        StartCoroutine(goesUp());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator goesUp()
    {
        while (distance > 0)
        {
            distance -= 0.1f;
            transform.localPosition += Vector3.up * 0.1f;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
