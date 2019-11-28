using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Ennemy
{
    // Start is called before the first frame update
    private Animator animator;
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        animator.SetFloat("X", (float)wDirection);
        base.Update();   
    }
}
