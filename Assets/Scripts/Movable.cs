using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Movable : MonoBehaviour
{
    protected DynamicGrid grid;
    [SerializeField]
    protected bool isMoving = false;
    [SerializeField]
    protected bool onCooldown = false;
    [SerializeField]
    private bool onExit = false;
    [Space]
    [SerializeField]
    protected float coolDown;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float currentSpeed;
    [SerializeField]
    protected Vector2 currentCell;

    // Use this for initialization
    protected virtual void Start()
    {
        grid = FindObjectOfType<DynamicGrid>();
        currentSpeed = speed;
        //Debug.Log("place in grid");
        //Para inimigos com multiplos sprites, trocar essa parte do codigo tal que peguemos
        //a posição de cada sprites separados. Vale notar que cas o inimigo tiver uma altura maior que 1
        //não queremos necessariamente adicionar este sprites no grid dinàmico.
        if (grid != null)
            currentCell = grid.placeInGrid(this.transform.position, this.gameObject);
    }

    protected void goTo(Vector2 movement)
    {
        //We do nothing if the player is still moving.
        if (isMoving || onCooldown || onExit) return;

        //We can't go in both directions at the same time

        if (movement.x != 0)
            movement.y = 0;


        //If there's a direction, we are trying to move.
        if (movement.x != 0 || movement.y != 0)
        {
            StartCoroutine(actionCooldown(coolDown));
            currentCell = Move(movement);
        }
    }

    private Vector2 Move(Vector2 movement)
    {
        Vector2 startCell = transform.position;
        Vector2 targetCell = startCell + movement;
        Vector2 res;
        //If the front tile is a walkable ground tile, the player moves here.
        if (grid.cellIsEmpty(targetCell))
        {
            //Para inimigos com multiplos sprites, trocar essa parte do codigo tal que peguemos
            //a posição de cada sprites separados. Vale notar que cas o inimigo tiver uma altura maior que 1
            //não queremos necessariamente adicionar este sprites no grid dinàmico.
            grid.moveInGrid(startCell, targetCell);
            StartCoroutine(SmoothMovement(targetCell));
            res = targetCell;
        }
        else
        {
            StartCoroutine(BlockedMovement(targetCell));
            res = startCell;
        }

        if (!isMoving)
            StartCoroutine(BlockedMovement(targetCell));

        return res;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, currentSpeed * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
    }

    //Blocked animation
    protected IEnumerator BlockedMovement(Vector3 end)
    {
        isMoving = true;

        Vector3 originalPos = transform.position;

        end = transform.position + ((end - transform.position) / 3);
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, currentSpeed/2 * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        sqrRemainingDistance = (transform.position - originalPos).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, originalPos, currentSpeed/2 * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - originalPos).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
    }
    protected IEnumerator actionCooldown(float cooldown)
    {
        onCooldown = true;

        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        onCooldown = false;
    }
}