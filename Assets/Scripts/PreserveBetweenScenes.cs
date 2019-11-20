using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveBetweenScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {/*
        DontDestroyOnLoad(this.gameObject);
        string preservedObjectTag = this.gameObject.tag;
        GameObject otherInstance = GameObject.FindWithTag(preservedObjectTag);
        if (!otherInstance.Equals(this.gameObject))
            Destroy(this.gameObject);*/
    }
}
