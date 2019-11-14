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
    [SerializeField]
    private PlayerInput playerInput;
    private CatsInput controller;

    private void Awake()
    {
        controller = new CatsInput();
        controller.Enable();
        controller.Player.Move.performed += context => walk(context.ReadValue<Vector2>());
        controller.Player.AttackA.performed += context => useAttackA();
        controller.Player.AttackB.performed += context => useAttackB();
        controller.Player.AttackX.performed += context => useAttackX();
        controller.Player.AttackY.performed += context => useAttackY();
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
        timeFromLastAttack += Time.deltaTime;

        if (timeFromLastAttack > comboTiming && currentCombo > 0)
            setCombo(0);

        if (currentMana <= 0)
        {
            setMana(0);
            StartCoroutine(ManaCoolDown(manaCoolDown));
        }
        if (!manaIsRegenarating)
        {
            //Attaques
            /*
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                lastAttack = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                lastAttack = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                lastAttack = 2;
            }*/
        }

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

            //Movimentação
            /*
            movement.x = (int)Input.GetAxisRaw("Horizontal");
            movement.y = (int)Input.GetAxisRaw("Vertical");*/

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
        goTo(movement);
    }

    private void useAttackA()
    {
        Debug.Log("AttackA");
        lastAttack = 0;
    }
    private void useAttackB()
    {
        Debug.Log("AttackB");
        lastAttack = 1;
    }
    private void useAttackX()
    {
        Debug.Log("AttackX");
        lastAttack = 2;
    }
    private void useAttackY()
    {
        Debug.Log("AttackY");
        lastAttack = 3;
    }

    private void walk(Vector2 movement)
    {
        this.movement.x = (int)movement.x;
        this.movement.y = (int)movement.y;
    }
}
