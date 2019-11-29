using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTorches : Puzzles
{
    [SerializeField]
    Torch[] torchesToLight;
    [SerializeField]
    bool checkOrder;
    int currentLight = 0;
    bool currentOrder = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < torchesToLight.Length; i++)
        {
            torchesToLight[i].stateChanged.AddListener(checkLights);
        }  
    }

    void checkLights()
    {
        if (checkOrder)
            if (!torchesToLight[currentLight].isActivated)
            {
                currentOrder = false;
            }
        currentLight++;
        if (currentLight >= torchesToLight.Length)
        {
            if (currentOrder)
                completed();
            else
                Reset();
        }
    }

    private void Reset()
    {
        Debug.Log("Reset");
        for (int i = 0; i < torchesToLight.Length; i++)
        {
            torchesToLight[i].setState(false);
        }
        currentLight = 0;
        currentOrder = true;
    }
}
