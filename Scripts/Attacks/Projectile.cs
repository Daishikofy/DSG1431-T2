using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Movable
{
    [SerializeField]
    private int range;
    [SerializeField]
    Vector2 direction;
    [SerializeField]
    Explosion explosion;
    private ProjectileAttack projectilController;
    private int damage;
    private void Start()
    { }
    public void Inicialize(Vector2 direction, int range, int power, ProjectileAttack projectilController)
    {
        currentSpeed = speed;
        grid = FindObjectOfType<DynamicGrid>();
        this.direction = direction;
        this.damage = power;
        this.range = range;
        this.projectilController = projectilController;
    }
    private void Update()
    {
        if (isMoving) return;
        bool cellIsEmpty = grid.cellIsEmpty(this.transform.position);
        if (range > 0 && cellIsEmpty)
        {
            Vector2 startCell = transform.position;
            Vector2 targetCell = startCell + direction;
            StartCoroutine(SmoothMovement(targetCell));

            range--;
        }
        else
        {
            if (!cellIsEmpty)
            {
                GameObject target = grid.getInCell(this.transform.position);
                if (target != null)
                    GiveDamages(target);
            }

            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void GiveDamages(GameObject target)
    {
        Fighter ennemy = target.GetComponent<Fighter>();
        ennemy.OnDamaged(damage);      
        projectilController.giveDamage(ennemy);
    }









    /*
    private bool isInicialized;
    private Vector2 direction;
    private int range;
    private int damage;

    [SerializeField]
    private GameObject explosion;
    private void Start()
    {
        base.Start();
        isInicialized = true;
    }
    // Update is called once per frame
    */
    /*
void Update()
    {
        Debug.Log("Passou aqui 4");
        direction = Vector2.up;
        if (!isInicialized || grid == null)
            return;

        Debug.Log("Passou aqui 5");
        while (range > 0)
        {
            Debug.Log("Passou aqui 6");
            Vector2 projectilePosition = grid.worldToDynamicGridCell(this.GetComponent<Transform>().position);
            Debug.Log("Range: " + range);
            goTo(direction);
            range -= 1;*/
    /*
    if (grid.cellIsEmpty(projectilePosition))
    {
        Debug.Log("Range: " + range);
        goTo(direction);
        range -= 1;
    }
    else
    {
        Instantiate(explosion, (Vector3)projectilePosition, Quaternion.identity);
        Destroy(this.gameObject, 2.0f);
    }*/
}
        /*Instantiate(explosion, (Vector3)projectilePosition, Quaternion.identity);*/
       /* Destroy(this.gameObject, 2.0f);
    }

    public void Inicialize(Vector2 direction, int range, int damage)
    {
        Debug.Log("Projectile Inicialize");
        //isInicialized = true;
        this.direction = direction;
        this.range = range;
        this.damage = damage;
        isInicialized = true;
    }
}*/
