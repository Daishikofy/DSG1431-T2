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
        currentCell = this.transform.position;
        grid = FindObjectOfType<DynamicGrid>();
        this.direction = direction;
        this.damage = power;
        this.range = range;
        this.projectilController = projectilController;
    }
    private void Update()
    {
        if (isMoving) return;
        bool cellIsEmpty = grid.cellIsEmpty(this.currentCell);
        if (range > 0 && cellIsEmpty)
        {
            Vector2 startCell = transform.position;
            Vector2 targetCell = startCell + direction;
            StartCoroutine(SmoothMovement(targetCell));
            currentCell = targetCell;
            range--;
        }
        else
        {
            if (!cellIsEmpty)
            {
                GameObject target = grid.getInCell(currentCell);
                if (target != null)
                {
                    Debug.Log("bam");
                    GiveDamages(target);
                }
            }

            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void GiveDamages(GameObject target)
    {
        var ennemy = target.GetComponent<IDamageable>();
        projectilController.giveDamage(ennemy);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(currentCell, 0.1f);
    }

}

