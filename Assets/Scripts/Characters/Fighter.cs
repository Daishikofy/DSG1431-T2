﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fighter : Movable
{
    [Space]
    [SerializeField]
    protected int maxLife;
    [SerializeField]
    protected int currentLife;

    [Space]
    [SerializeField]
    protected int maxMana;
    public int currentMana;
    [SerializeField]
    protected float manaCoolDown;
    [SerializeField]
    protected bool manaIsRegenarating;

    [Space]
    [SerializeField]
    protected int magicPower;
    [SerializeField]
    protected int physicPower;

    [Space]
    [SerializeField]
    protected int combo;
    public int currentCombo;
    [SerializeField]
    protected float timeFromLastAttack;
    [SerializeField]
    protected float comboTiming;

    [Space]
    [SerializeField]
    private float stagger = 0.25f;
    private bool stagged = false;
    [Space]
    [SerializeField]
    public Attack[] moveSet; // TODO: Botar privado novamente (vai quebrar no nivel do panel de attaques)
    [Space]

    [SerializeField]
    public IntEvent updateLifeUI;
    [SerializeField]
    public IntEvent updateManaUI;
    [SerializeField]
    public IntEvent updateComboUI;
    protected Vector2Int direction;
    protected Vector2Int movement;

    private bool isKo = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        setMana(maxMana);
        setLife(maxLife);
        setCombo(0);
        direction = Vector2Int.down;
        base.Start();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentLife <= 0 && !isKo)
        {
            KnockOut();
        }
        else if (!isMoving && !onCooldown)
            if (stagged)
            {
                stagged = false;
                StartCoroutine(actionCooldown(stagger + coolDown));     
            }
    }

    protected IEnumerator ManaCoolDown(float coolDown)
    {
        manaIsRegenarating = true;
        float time = coolDown / maxMana;

        while (currentMana < maxMana)
        {
            addMana(1);
            yield return new WaitForSecondsRealtime(time);
            
            yield return null;
        }

        manaIsRegenarating = false;
    }

    public virtual void OnDamaged(int damage)
    {
        //TODO animator: Blink
        addLife(damage * -1);
        stagged = true;
    }

    private void KnockOut()
    {
        setLife(0);
        isKo = true;
        //For Debug purpose
        GetComponent<SpriteRenderer>().color = Color.black;
        //TODO - Animator: deathAnimation
        Debug.Log("Knock out: " + this.gameObject.name);
        grid.removeFromGrid(currentCell);
        Destroy(this.gameObject, 1.0f);
        //IMPORTANT!! REMOVE THIS LINE ABOVE
    }

    public Vector2 getDirection()
    {
        return direction;
    }

    public void addMana(int mana)
    {
        setMana(currentMana + mana);
    }
    public void setMana(int mana)
    {
        currentMana = mana;
        updateManaUI.Invoke(mana);
    }

    public void addLife(int life)
    {
        setLife(currentLife + life);
    }
    public void setLife(int life)
    {
        currentLife = life;
        updateLifeUI.Invoke(life);
    }
    public void addCombo()
    {
        setCombo(currentCombo+=1);
    }
    public void setCombo(int combo)
    {
        currentCombo = combo;
        updateComboUI.Invoke(currentCombo);
    }
}
