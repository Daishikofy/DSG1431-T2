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
                Reset();
                return;
            }
        currentLight++;
        if (currentLight == torchesToLight.Length)
            completed();
    }

    private void Reset()
    {
        for (int i = 0; i < torchesToLight.Length; i++)
        {
            torchesToLight[i].setState(false);
        }
        currentLight = 0;
    }
}
