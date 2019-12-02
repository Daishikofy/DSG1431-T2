//using System.Drawing;
using UnityEngine;

public class MovementControl : Movable
{
    public int AttackRange;
    public int SeekRange;
    public Vector2 PatrolWalkDistance;
    public Vector2 PatrolStartPosition;
    public Vector2 BaseLeft;
    public Vector2 TopRight;
    public PatrolDirection startDirection;

    public Vector2Int currentPos;
    public Vector2Int playerPos;

    private PathFinding controlador;
    private GameObject player;

    private float pathFindingCooldown;
    private bool onPathFindingCooldown = false;
    protected override void Start()
    {
        var patrolWalkDistance = new Vector2Int((int)PatrolWalkDistance.x, (int)PatrolWalkDistance.y);
        var patrolStartPosition = new Vector2Int((int)PatrolStartPosition.x, (int)PatrolStartPosition.y);
        var baseLeft = new Vector2Int((int)BaseLeft.x, (int)BaseLeft.y);
        var topRight = new Vector2Int((int)TopRight.x, (int)TopRight.y);           
            
        controlador = new PathFinding(AttackRange, SeekRange, patrolWalkDistance, patrolStartPosition, baseLeft, topRight, startDirection);
        player = GameObject.FindGameObjectWithTag("Player");

        pathFindingCooldown = 0.2f;
        onPathFindingCooldown = false;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!onPathFindingCooldown)
        //{
        if (!isMoving && !onCooldown)
        {
            onPathFindingCooldown = true;
            pathFindingCooldown = 0.2f;
            currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
            Vector2Int direction = controlador.DefineDirection(grid, currentPos, playerPos);
            if (direction.x != -1 || direction.y != -1)
            {
                if (controlador.State == MovementState.Attack)
                    Debug.Log("Ataquei");
                else
                    goTo(new Vector2(direction.x, direction.y));

            }
        }
        //}
        //else
        //{
        //    pathFindingCooldown -= Time.deltaTime;
        //    if (pathFindingCooldown <= 0)
        //        onPathFindingCooldown = false;
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(BaseLeft, 0.1f);
        Gizmos.DrawSphere(TopRight, 0.1f);
        var topLeft = new Vector2(BaseLeft.x, TopRight.y);
        Gizmos.DrawSphere(topLeft, 0.1f);
        var baseRight = new Vector2(TopRight.x, BaseLeft.y);
        Gizmos.DrawSphere(baseRight, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(TopRight, topLeft);
        Gizmos.DrawLine(TopRight, baseRight);
        Gizmos.DrawLine(BaseLeft, topLeft);
        Gizmos.DrawLine(BaseLeft, baseRight);
    }
}
