using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleEnnemies : Puzzles
{
    [SerializeField]
    Fighter[] ennemies;
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ennemies.Length; i++)
        {
            if (ennemies[i].isActiveAndEnabled)
                return;
        }
        completed();
    }
}
