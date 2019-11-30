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
    protected int wDirection;
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
        if (isKo) return;
        base.Update();
        Fighter obj;
        if (axe == 'x')
        {
            if (gameObject.transform.position.x > minMax.y)
                wDirection = -1;
            else if (gameObject.transform.position.x < minMax.x)
                wDirection = 1;
            obj = goTo(Vector2.right * wDirection);
        }
        else
        {
            if (gameObject.transform.position.y > minMax.y)
                wDirection = -1;
            else if (gameObject.transform.position.y < minMax.x)
                wDirection = 1;
            obj = goTo(Vector2.up * wDirection);
        }
        if (obj != null && obj.CompareTag("Player"))
            obj.OnDamaged(1, Element.None);
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
