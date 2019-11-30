using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Movable : MonoBehaviour
{
    protected DynamicGrid grid;
    // [SerializeField]
    public bool isMoving = false; //TODO:Set as protected and implement exitDoor in player's script
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

    public Vector2 currentCell;

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

    protected Fighter goTo(Vector2 movement)
    {
        //We do nothing if the player is still moving.
        if (isMoving || onCooldown || onExit) return null;

        //We can't go in both directions at the same time

        if (movement.x != 0)
            movement.y = 0;


        //If there's a direction, we are trying to move.
        if (movement.x != 0 || movement.y != 0)
        {
            StartCoroutine(actionCooldown(coolDown));
            Fighter blockingCharacter = Move(movement);
            return blockingCharacter;
        }
        return null;
    }

    private Fighter Move(Vector2 movement)
    {
        Vector2 startCell = transform.position;
        Vector2 targetCell = startCell + movement;
        GameObject res;
        //If the front tile is a walkable ground tile, the player moves here.
        if (grid.cellIsEmpty(targetCell))
        {
            //Para inimigos com multiplos sprites, trocar essa parte do codigo tal que peguemos
            //a posição de cada sprites separados. Vale notar que cas o inimigo tiver uma altura maior que 1
            //não queremos necessariamente adicionar este sprites no grid dinàmico.
            grid.moveInGrid(startCell, targetCell);
            StartCoroutine(SmoothMovement(targetCell));
            currentCell = targetCell;
            res = null;
        }
        else
        {
            StartCoroutine(BlockedMovement(targetCell));
            currentCell = startCell;
            res = grid.getInCell(targetCell);
        }

        if (!isMoving)
            StartCoroutine(BlockedMovement(targetCell));
        if (res == null)
            return null;
        return res.GetComponent<Fighter>();
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

    private void OnDrawGizmos()
    {
        Vector2 pos = currentCell + (Vector2.one * 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.1f);
    }
}