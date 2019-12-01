using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    public Point currentPos;
    public Point playerPos;

    private PathFinding controlador;
    private GameObject player;

    private float pathFindingCooldown;
    private bool onPathFindingCooldown = false;
    protected override void Start()
    {
        Point patrolWalkDistance = new Point((int)PatrolWalkDistance.x, (int)PatrolWalkDistance.y);
        Point patrolStartPosition = new Point((int)PatrolStartPosition.x, (int)PatrolStartPosition.y);
        Point baseLeft = new Point((int)BaseLeft.x, (int)BaseLeft.y);
        Point topRight = new Point((int)TopRight.x, (int)TopRight.y);           
            
        controlador = new PathFinding(AttackRange, SeekRange, patrolWalkDistance, patrolStartPosition, baseLeft, topRight, startDirection);
        player = GameObject.FindGameObjectWithTag("Player");

        pathFindingCooldown = 0.2f;
        onPathFindingCooldown = false;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onPathFindingCooldown)
        {
            if (!isMoving && !onCooldown)
            {
                onPathFindingCooldown = true;
                pathFindingCooldown = 0.2f;
                currentPos = new Point((int)transform.position.x, (int)transform.position.y);
                playerPos = new Point((int)player.transform.position.x, (int)player.transform.position.y);
                Point direction = controlador.DefineDirection(grid, currentPos, playerPos);
                if (direction.X != 0 || direction.Y != 0)
                    goTo(new Vector2(direction.X, direction.Y));
            }
        }
        else
        {
            pathFindingCooldown -= Time.deltaTime;
            if (pathFindingCooldown <= 0)
                onPathFindingCooldown = false;
        }
    }
}
