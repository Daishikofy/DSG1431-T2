using UnityEngine;

public class Ghost : MovementControl
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
        if (direction.x != 0)
            animator.SetFloat("X", (float)direction.x);
        base.Update();   
    }
}
