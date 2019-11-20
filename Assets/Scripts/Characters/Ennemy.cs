using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Fighter
{

    [SerializeField]
    private Vector2 minMax;
    [SerializeField]
    private char axe;
    [SerializeField]
    private float markedTime;

    private float timer;

    private SpriteRenderer renderer;
    private int wDirection;
    private Color color;
    // Use this for initialization
    protected override void Start()
    {
        wDirection = -1;
        renderer = GetComponent<SpriteRenderer>();
        color = renderer.color;
        timer = markedTime * 60;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (axe == 'x')
        {
            if (gameObject.transform.position.x > minMax.y)
                wDirection = -1;
            else if (gameObject.transform.position.x < minMax.x)
                wDirection = 1;
            goTo(Vector2.right * wDirection);
        }
        else
        {
            if (gameObject.transform.position.y > minMax.y)
                wDirection = -1;
            else if (gameObject.transform.position.y < minMax.x)
                wDirection = 1;
            goTo(Vector2.up * wDirection);
        }
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            renderer.color = color;
            timer = markedTime * 60;
        }
        if (renderer.color != color && timer > 0)
        {
            timer--;
        }
    }
}
