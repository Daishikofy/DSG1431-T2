using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.InputSystem;

public class Player : Fighter
{
    private Animator animator;
    private int lastAttack;   
    private CatsInput controller;
    private int stop = 0;
    public Inventory inventory;

    [SerializeField]
    private float rechargeTime = 10.0f;
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        controller = new CatsInput();
        controller.Player.Enable();
        controller.Player.X.performed += context => horizontal(context.ReadValue<float>());
        controller.Player.Y.performed += context => vertical(context.ReadValue<float>());
        controller.Player.AttackA.performed += context => useAttackA();
        controller.Player.AttackB.performed += context => useAttackB();
        controller.Player.AttackX.performed += context => useAttackX();
        controller.Player.AttackY.performed += context => useAttackY();
		controller.Player.Interact.performed += context => interact();
    }
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        lastAttack = -1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (timeFromLastAttack <= rechargeTime + 1)
            timeFromLastAttack += Time.deltaTime;

        if (timeFromLastAttack > comboTiming && currentCombo > 0)
            setCombo(0);
        if (timeFromLastAttack >= rechargeTime && currentMana < maxMana)
            setMana(maxMana);
        if (currentMana <= 0)
        {
            setMana(0);
            StartCoroutine(ManaCoolDown(manaCoolDown));
        }
        if (manaIsRegenarating)
        {
            lastAttack = -1;
        }
        if (attackCoolDown)
            return;

        if (!isMoving)
        {
            if (lastAttack >= 0 && !manaIsRegenarating)
            {
                if (currentCombo >= combo)
                {
                    Debug.Log("COMBO!!!");
                    setCombo(0);
                    //TODO: Implemente combo effects
                }
                if (moveSet.Length > lastAttack)
                {
                    moveSet[lastAttack].use(this);
                    timeFromLastAttack = 0;
                }
                lastAttack = -1;           
            }
        }

        if (movement.x != 0)
            movement.y = 0;

        if (movement.x != 0 || movement.y != 0)
        {
            direction = movement;           
            if (!isMoving)
            {
                animator.SetFloat("Move X", direction.x);
                animator.SetFloat("Move Y", direction.y);
            }
        }
        animator.SetBool("Moving", isMoving);
        if (stop <= 0)
            goTo(movement);
        else
            stop--;
    }

    public void changeLevel(Vector2 position)
    {
        currentCell = position;
        transform.position = position;
        grid.placeInGrid(currentCell, this.gameObject);
    }

    public void stopForFrames(int frames)
    {
        stop = frames;
    }

    private void useAttackA()
    {
        lastAttack = 0;
    }
    private void useAttackB()
    {
        Debug.Log("attack:" + moveSet[1].name);
        lastAttack = 1;
    }
    private void useAttackX()
    {
        lastAttack = 2;
		this.setMana(0);
		this.setLife(this.maxLife);
    }
    private void useAttackY()
    {
        lastAttack = 3;
    }
	
	private void interact()
	{
        Vector2 frontCell = currentCell + direction;
        GameObject obj = grid.getInCell(frontCell);
        if (obj != null)
        {
            obj.GetComponent<InterfaceInteractiveObject>().onInteraction(this);
        }
	}

    private void horizontal(float value)
    {
        if (value == 0 && movement.y != 0)
            return;
        this.movement.x = (int)value;
        this.movement.y = 0;
    }

    private void vertical (float value)
    {
        if (value == 0 && movement.x != 0)
            return;
        this.movement.x = 0;
        this.movement.y = (int)value;
    }
    private void walk(Vector2 movement)
    {
        this.movement.x = (int)movement.x;
        this.movement.y = (int)movement.y;
    }

    public void addToInventory(string name)
    {
        inventory.addItem(name);
    }
}
