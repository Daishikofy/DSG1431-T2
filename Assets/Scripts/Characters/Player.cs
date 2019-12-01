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
    [HideInInspector]
    public Inventory inventory;
    private bool changeDirection = false;

    [SerializeField]
    private float rechargeTime = 10.0f;
    [SerializeField]
    private CharacterManager characterManager;
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
        controller.Player.PrepAttack.performed += context => prepAttack();
    }
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        characterManager.changeSkin("witche", this);
        lastAttack = -1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKo) return;
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
        {
            Fighter obj;
            obj = goTo(movement);
            if (obj != null && obj.CompareTag("Ennemy"))
                OnDamaged(1, Element.None);
        }
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

    public void reset()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        currentCell = new Vector2(4, 5);
        transform.position = currentCell;
        grid.placeInGrid(currentCell, this.gameObject);
        if (isKo)
            isKo = false;
        setLife(maxLife);
        setMana(maxMana);
    }

    public void enableController(bool value)
    {
        if (value)
            controller.Enable();
        else
            controller.Disable();
    }

    public void restrictController(bool value)
    {
        if (value)
        {
            controller.Player.AttackA.Disable();
            controller.Player.AttackB.Disable();
            controller.Player.AttackX.Disable();
            controller.Player.AttackY.Disable();
        }
        else
        {
            controller.Player.AttackA.Enable();
            controller.Player.AttackB.Enable();
            controller.Player.AttackX.Enable();
            controller.Player.AttackY.Enable();
        }
    }

    private void useAttackA()
    {
        lastAttack = 0;
    }
    private void useAttackB()
    {
        lastAttack = 1;
    }
    private void useAttackX()
    {
        lastAttack = 2;
    }
    private void useAttackY()
    {
        lastAttack = 3;
    }
	
	private void interact()
	{
       // Debug.Log("Interacting");
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

        if (changeDirection && value != 0)
        {
            this.direction.x = (int)value;
            this.direction.y = 0;
            animator.SetFloat("Move X", direction.x);
            animator.SetFloat("Move Y", direction.y);
        }
        else
        {
            this.movement.x = (int)value;
            this.movement.y = 0;
        }
    }

    private void vertical (float value)
    {
        if (value == 0 && movement.x != 0)
            return;
        if (changeDirection && value != 0)
        {
            this.direction.x = 0;
            this.direction.y = (int)value;
            animator.SetFloat("Move X", direction.x);
            animator.SetFloat("Move Y", direction.y);
        }
        else
        {
            this.movement.x = 0;
            this.movement.y = (int)value;
        }
    }
    private void walk(Vector2 movement)
    {
        this.movement.x = (int)movement.x;
        this.movement.y = (int)movement.y;
    }

    private void prepAttack()
    {
        changeDirection = !changeDirection;
    }

    public void addToInventory(string name)
    {
        inventory.addItem(name);
    }
}
