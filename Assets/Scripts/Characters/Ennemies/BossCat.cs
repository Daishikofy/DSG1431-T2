using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCat : MovementControl
{
    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (direction.x != 0)
        {
            animator.SetFloat("Move X", (float)direction.x);
            animator.SetFloat("Move Y", 0f);
        }
        else if (direction.y != 0)
        {
            animator.SetFloat("Move Y", (float)direction.y);
            animator.SetFloat("Move X", 0f);
        }

        base.Update();
    }
}
