using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrow : Fighter
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLife < maxLife)
            currentLife = maxLife;
    }

    public override void OnDamaged(int damage)
    {
        GetComponent<Animator>().SetTrigger("Damaged");
        base.OnDamaged(damage);
    }
}
