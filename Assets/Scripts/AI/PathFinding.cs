using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;
using UnityEngine;

public class PathFinding
{

    public Point Direction;

    public int AttackRange;
    public int SeekRange;
    public MovementState State;

    private Point BaseLeftPos;
    private Point CurrentPos;
    private Point CurrentRealPos;
    private Point PlayerPos;
    private Point PlayerRealPos;
    private Point LastPlayerPos;

    private Point PatrolStartPosition;
    private Point PatrolWalkDistance;
    private PatrolDirection PatrolStartDirection;
    private PatrolDirection PatrolCurrentDirection;

    private int tamGridX;
    private int tamGridY;

    private IList<IList<Node>> NodesGrid;
    private IList<Node> Path;
    private bool followingAStar = false;

    private System.Random Rand;

    private Point VectorToPoint(Vector2Int vector)
    {
        var point = new Point(vector.x, vector.y);
        return point;
    }
    public PathFinding(int attackRange
                      ,int seekRange
                      , Vector2Int _patrolWalkDistance
                      , Vector2Int _patrolStartPosition
                      , Vector2Int _baseLeft
                      , Vector2Int _topRight
                      , PatrolDirection startDirection)
    {
        Point patrolWalkDistance = VectorToPoint(_patrolWalkDistance);
        Point patrolStartPosition = VectorToPoint(_patrolStartPosition);
        Point baseLeft = VectorToPoint(_baseLeft);
        Point topRight = VectorToPoint(_topRight);

        AttackRange = attackRange;
        SeekRange = seekRange;
        State = MovementState.Patrol;

        BaseLeftPos = baseLeft;

        PatrolStartPosition = new Point(patrolStartPosition.X - BaseLeftPos.X, patrolStartPosition.Y - BaseLeftPos.Y);
        PatrolWalkDistance = patrolWalkDistance;
        PatrolStartDirection = startDirection;
        PatrolCurrentDirection = startDirection;

        tamGridX = Math.Abs(baseLeft.X - topRight.X) + 1;
        tamGridY = Math.Abs(baseLeft.Y - topRight.Y) + 1;
        
        Rand = new System.Random();
    }

    public Vector2Int DefineDirection(DynamicGrid grid, Vector2Int _currentPos, Vector2Int _playerPos)
    {
        var currentPos = VectorToPoint(_currentPos);
        var playerPos = VectorToPoint(_playerPos);

        var lastPlayerPos = PlayerPos;
        var lastState = State;
        bool wasChassingPlayer = State == MovementState.MoveAttackPosition || State == MovementState.Seek;

        CurrentRealPos = currentPos;
        PlayerRealPos = playerPos;
        CurrentPos = new Point(currentPos.X - BaseLeftPos.X, currentPos.Y - BaseLeftPos.Y);
        PlayerPos = new Point(playerPos.X - BaseLeftPos.X, playerPos.Y - BaseLeftPos.Y);

        SetState();

        bool isChassingPlayer = State == MovementState.MoveAttackPosition || State == MovementState.Seek;

        if (followingAStar &&
            ((wasChassingPlayer && isChassingPlayer && lastPlayerPos.X == PlayerPos.X && lastPlayerPos.Y == PlayerPos.Y)
            || (State == MovementState.Return && lastState == MovementState.Return)))
        {
            var direction = ContinuePath();
            if (!PointIsInvalid(direction))
                return new Vector2Int(direction.X, direction.Y);
        }

        followingAStar = false;

        if (State == MovementState.Attack)
        {
            Point attackPosition = AttackPosition();
            return new Vector2Int(attackPosition.X, attackPosition.Y);
        }
        else if (State == MovementState.Patrol)
        {
            Point patrolMovement = PatrolMovement(); 
            return new Vector2Int(patrolMovement.X, patrolMovement.Y);
        }
        else
        {
            Point direction, destination;

            if (State == MovementState.Seek || State == MovementState.MoveAttackPosition)
                destination = PlayerPos;
            else// if (State == MovementState.Return)
            {
                destination = PatrolStartPosition;
                PatrolCurrentDirection = PatrolStartDirection;
            }

            InstantiateGrid(grid, destination);

            if (State == MovementState.MoveAttackPosition)
                direction = MoveToAttackPosition();
            else
                direction = TwoWayMovement(CurrentPos, destination);

            if (PointIsInvalid(direction))
            {
                direction = AStar(destination);
                if (!PointIsInvalid(direction))
                    followingAStar = true;
            }

            return new Vector2Int(direction.X, direction.Y);
        }

    }

    private void InstantiateGrid(DynamicGrid grid, Point destination)
    {
        NodesGrid = new List<IList<Node>>();

        for (int i = 0; i < tamGridX; i++)
        {
            NodesGrid.Add(new List<Node>());
            for (int j = 0; j < tamGridY; j++)
            {
                var realLocation = new Vector2(i + BaseLeftPos.X, j + BaseLeftPos.Y);
                Node node = new Node()
                {
                    ParentNode = null,
                    G = Math.Abs(CurrentPos.X - i) + Math.Abs(CurrentPos.Y - j),
                    H = Math.Abs(destination.X - i) + Math.Abs(destination.Y - j),
                    State = NodeState.Open,
                    Location = new Point(i, j),
                    RealLocation = realLocation,
                    IsWalkable = grid.cellIsEmpty(realLocation)
                };

                NodesGrid[i].Add(node);
            }
        }

    }

    private void SetState()
    {
        var distanceX = Math.Abs(CurrentPos.X - PlayerPos.X);
        var distanceY = Math.Abs(CurrentPos.Y - PlayerPos.Y);

        if(State == MovementState.Return && CurrentPos.X == PatrolStartPosition.X && CurrentPos.Y == PatrolStartPosition.Y)
            State = MovementState.Patrol;
        else if (PlayerPos.X >= tamGridX || PlayerPos.X < 0 || PlayerPos.Y >= tamGridY || PlayerPos.Y < 0)
            State = State != MovementState.Patrol ? MovementState.Return : MovementState.Patrol;
        else if (distanceX > SeekRange || distanceY > SeekRange)
            State = State != MovementState.Patrol ? MovementState.Return : MovementState.Patrol;
        else if (distanceX > AttackRange || distanceY > AttackRange)
            State = MovementState.Seek;
        else if (distanceX > 0 && distanceY > 0)
            State = MovementState.MoveAttackPosition;
        else
        {
            Point testAttack;
            if(distanceX == 0)
                testAttack = StraightLineMovement(CurrentPos, PlayerPos, false, true);
            else
                testAttack = StraightLineMovement(CurrentPos, PlayerPos, true, true);

            if (PointIsInvalid(testAttack))
                State = MovementState.MoveAttackPosition;
            else
                State = MovementState.Attack;
        }
        /*
        else if (distanceX == 0)
        {
            if (currentPos.X > playerPos.X)
                State = MovementState.LeftAttack;
            else
                State = MovementState.RightAttack;
        }
        else
        {
            if (currentPos.y > playerPos.y)
                State = MovementState.DownAttack;
            else
                State = MovementState.UpAttack;
        }
        */
    }

    private Point TwoWayMovement(Point startPos, Point destination)
    {
        bool moveInXFirst = Rand.Next() % 2 == 0;

        Point firstDestination1, firstDestination2;
        if (moveInXFirst)
        {
            firstDestination1 = new Point(destination.X, startPos.Y);
            firstDestination2 = new Point(startPos.X, destination.Y);
        }
        else
        {
            firstDestination1 = new Point(startPos.X, destination.Y);
            firstDestination2 = new Point(destination.X, startPos.Y);
        }

        Point direction = StraightLineMovement(startPos, firstDestination1, moveInXFirst, false);
        if (!PointIsInvalid(direction))
        {
            if(firstDestination1.X == destination.X && firstDestination1.Y == destination.Y)
                return direction;
            if (!PointIsInvalid(StraightLineMovement(firstDestination1, destination, !moveInXFirst, true)))
                return direction;
        }

        direction = StraightLineMovement(startPos, firstDestination2, !moveInXFirst, false);
        if (!PointIsInvalid(direction))
        {
            if (firstDestination2.X == destination.X && firstDestination2.Y == destination.Y)
                return direction;
            if (!PointIsInvalid(StraightLineMovement(firstDestination2, destination, moveInXFirst, true)))
                return direction;
        }

        return new Point(-1, -1);
    }

    private Point StraightLineMovement(Point startPos, Point destination, bool moveInX, bool ignoreObstaculeAtDestination)
    {
        if (startPos.X == destination.X && startPos.Y == destination.Y)
            return new Point(-1, -1);

        int curPosInAxys, destinationPosInAxys;
        if (moveInX)
        {
            curPosInAxys = startPos.X;
            destinationPosInAxys = destination.X;
        }
        else
        {
            curPosInAxys = startPos.Y;
            destinationPosInAxys = destination.Y;
        }

        int movement = curPosInAxys > destinationPosInAxys ? -1 : 1;
                
        while (curPosInAxys != destinationPosInAxys)
        {
            bool isWalkable;
            curPosInAxys += movement;

            if (ignoreObstaculeAtDestination && curPosInAxys == destinationPosInAxys)
                break;

            if (moveInX)
                isWalkable = NodesGrid[curPosInAxys][startPos.Y].IsWalkable;
            else
                isWalkable = NodesGrid[startPos.X][curPosInAxys].IsWalkable;

            if (!isWalkable)
                return new Point(-1, -1);
        }

        if (moveInX)
            return new Point(movement, 0);

        return new Point(0, movement);
    }

    private bool PointIsInvalid(Point p)
    {
        if (p.X == -1 && p.Y == -1)
            return true;
        return false;
    }

    private Point ContinuePath()
    {
        if(Path.Count < 2)
            return new Point(-1, -1);
        Path.Remove(Path.FirstOrDefault());
        foreach (var node in Path)
        {
            if (!NodesGrid[node.Location.X][node.Location.Y].IsWalkable)
                return new Point(-1, -1);
        }

        var destination = Path.FirstOrDefault().Location;
        int dirX, dirY;

        if (CurrentPos.X > destination.X)
            dirX = -1;
        else if(CurrentPos.X < destination.X)
            dirX = 1;
        else
            dirX = 0;

        if (CurrentPos.Y > destination.Y)
            dirY = -1;
        else if (CurrentPos.Y < destination.Y)
            dirY = 1;
        else
            dirY = 0;

        var direction = new Point(dirX, dirY);
        return direction;
    }

    private Point AttackPosition()
    {
        return new Point(PlayerPos.X - CurrentPos.X, PlayerPos.Y - CurrentPos.Y);
    }

    #region A Star

    public Point AStar(Point destination)
    {
        if (destination.X == CurrentPos.X && destination.Y == CurrentPos.Y)
            return new Point(-1, -1);

        Node currentNode = NodesGrid[CurrentPos.X][CurrentPos.Y];

        if (Search(currentNode, destination))
        {
            Point nextTile = Path.FirstOrDefault().Location;
            var direction = new Point(nextTile.X - CurrentPos.X, nextTile.Y - CurrentPos.Y);
            return direction;
        }
        else
            return new Point(-1, -1);
    }
       
    private void GeraPath(Node finalNode)
    {
        Path = new List<Node>();
        Node node = finalNode;

        while(node.Location.X != CurrentPos.X || node.Location.Y != CurrentPos.Y)
        {
            Path.Add(node);
            node = node.ParentNode;
        }

        Path = Path.Reverse().ToList();
        Path.Remove(finalNode);
    }

    private bool Search(Node currentNode, Point destination)
    {
        currentNode.State = NodeState.Closed;
        List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode, destination);
        nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
        foreach (var nextNode in nextNodes)
        {
            if (nextNode.Location == destination)
            {
                GeraPath(nextNode);
                return true;
            }
            else
            {
                if (Search(nextNode, destination)) // Note: Recurses back into Search(Node)
                    return true;
            }
        }
        return false;
    }

    private List<Node> GetAdjacentWalkableNodes(Node fromNode, Point destination)
    {
        List<Node> walkableNodes = new List<Node>();
        IList<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in nextLocations)
        {
            int x = location.X;
            int y = location.Y;
            
            Node node = NodesGrid[x][y];

            if (x == destination.X && y == destination.Y)
            {
                node.ParentNode = fromNode;
                walkableNodes.Add(node);
                return walkableNodes;
            }

            // Ignore non-walkable nodes
            if (!node.IsWalkable)
                continue;

            // Ignore already-closed nodes
            if (node.State == NodeState.Closed)
                continue;

            node.ParentNode = fromNode;
            walkableNodes.Add(node);
        }

        return walkableNodes;
    }

    private List<Point> GetAdjacentLocations(Point location)
    {
        List<Point> adjacentNodes = new List<Point>();

        int locationX = location.X;
        int locationY = location.Y;

        if (locationX + 1 < tamGridX)
            adjacentNodes.Add(NodesGrid[locationX + 1][locationY].Location);
        if (locationX - 1 >= 0)
            adjacentNodes.Add(NodesGrid[locationX - 1][locationY].Location);
        if (locationY + 1 < tamGridY)
            adjacentNodes.Add(NodesGrid[locationX][locationY + 1].Location);
        if (locationY - 1 >= 0)
            adjacentNodes.Add(NodesGrid[locationX][locationY - 1].Location);


        return adjacentNodes;
    }
    #endregion

    #region Move to Attack Position

    public Point MoveToAttackPosition()
    {
        Point destination, secondaryDestination;
        bool moveEixoX;

        if (PlayerPos.X - CurrentPos.X == PlayerPos.Y - CurrentPos.Y)
            moveEixoX = Rand.Next() % 2 == 0;
        else if (PlayerPos.X - CurrentPos.X > PlayerPos.Y - CurrentPos.Y)
            moveEixoX = false;
        else
            moveEixoX = true;
        
        if (moveEixoX)
        {
            destination = new Point(PlayerPos.X, CurrentPos.Y);
            secondaryDestination = new Point(CurrentPos.X, PlayerPos.Y);
        }
        else
        {
            destination = new Point(CurrentPos.X, PlayerPos.Y);
            secondaryDestination = new Point(PlayerPos.X, CurrentPos.Y);
        }

        Point direction = StraightLineMovement(CurrentPos, destination, moveEixoX, false);
        Point testAttack = StraightLineMovement(destination, PlayerPos, !moveEixoX, true);
        if (PointIsInvalid(direction) || PointIsInvalid(testAttack))
        {
            direction = StraightLineMovement(CurrentPos, secondaryDestination, !moveEixoX, false);
            testAttack = StraightLineMovement(secondaryDestination, PlayerPos, moveEixoX, true);
            if (PointIsInvalid(testAttack))
                return testAttack;
        }

        return direction;
    }

    #endregion

    #region Patrol Movement
    public Point PatrolMovement()
    {
        bool startedMovingLeft = PatrolStartDirection == PatrolDirection.LeftDown || PatrolStartDirection == PatrolDirection.LeftUp 
            || PatrolStartDirection == PatrolDirection.DownLeft || PatrolStartDirection == PatrolDirection.UpLeft;
        bool startedMovingUp = PatrolStartDirection == PatrolDirection.LeftUp || PatrolStartDirection == PatrolDirection.RightUp
            || PatrolStartDirection == PatrolDirection.UpLeft || PatrolStartDirection == PatrolDirection.UpRight;


        bool isMovingLeft = PatrolCurrentDirection == PatrolDirection.LeftDown || PatrolCurrentDirection == PatrolDirection.LeftUp;
        bool isMovingRight = PatrolCurrentDirection == PatrolDirection.RightDown || PatrolCurrentDirection == PatrolDirection.RightUp;
        bool isMovingUp = PatrolCurrentDirection == PatrolDirection.UpLeft || PatrolCurrentDirection == PatrolDirection.UpRight;
        bool isMovingDown = PatrolCurrentDirection == PatrolDirection.DownLeft || PatrolCurrentDirection == PatrolDirection.DownRight;

        if (isMovingDown)
        {
            if (!startedMovingUp)
            {
                if (CurrentPos.Y - 1 >= PatrolStartPosition.Y - PatrolWalkDistance.Y)
                    return new Point(0, -1);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
            else
            {
                if (CurrentPos.Y - 1 >= PatrolStartPosition.Y)
                    return new Point(0, -1);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
        }
        else if (isMovingUp)
        {
            if (startedMovingUp)
            {
                if (CurrentPos.Y + 1 <= PatrolStartPosition.Y + PatrolWalkDistance.Y)
                    return new Point(0, 1);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
            else
            {
                if (CurrentPos.Y + 1 <= PatrolStartPosition.Y)
                    return new Point(0, 1);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
        }
        else if (isMovingLeft)
        {
            if (startedMovingLeft)
            {
                if (CurrentPos.X - 1 >= PatrolStartPosition.X - PatrolWalkDistance.X)
                    return new Point(-1, 0);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
            else
            {
                if (CurrentPos.X - 1 >= PatrolStartPosition.X)
                    return new Point(-1, 0);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
        }
        else if (isMovingRight)
        {
            if (!startedMovingLeft)
            {
                if (CurrentPos.X + 1 <= PatrolStartPosition.X + PatrolWalkDistance.X)
                    return new Point(1, 0);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
            else
            {
                if (CurrentPos.X + 1 <= PatrolStartPosition.X)
                    return new Point(1, 0);
                else
                {
                    SetPatrolCurrentDirectionAsNext();
                    return PatrolMovement();
                }
            }
        }
        return new Point(-1, -1);
    }

    private void SetPatrolCurrentDirectionAsNext()
    {
        PatrolCurrentDirection = PatrolNextDirection(PatrolCurrentDirection);
    }

    private PatrolDirection PatrolNextDirection(PatrolDirection currentDirection)
    {
        if (currentDirection == PatrolDirection.DownLeft)
            return PatrolDirection.LeftUp;
        if (currentDirection == PatrolDirection.DownRight)
            return PatrolDirection.RightUp;
        if (currentDirection == PatrolDirection.LeftDown)
            return PatrolDirection.DownRight;
        if (currentDirection == PatrolDirection.LeftUp)
            return PatrolDirection.UpRight;
        if (currentDirection == PatrolDirection.RightDown)
            return PatrolDirection.DownLeft;
        if (currentDirection == PatrolDirection.RightUp)
            return PatrolDirection.UpLeft;
        if (currentDirection == PatrolDirection.UpLeft)
            return PatrolDirection.LeftDown;
        //if (currentDirection == PatrolDirection.UpRight)
            return PatrolDirection.RightDown;
    }
    #endregion
}

public class Node
{
    public float G; //distanceFromStart
    public float H; //straightLineDistanceToFinish
    public float F { get { return G + H; } set { } } //totalDistance

    public NodeState State;
    public bool IsWalkable;
    public Node ParentNode;
    public Point Location;
    public Vector2 RealLocation;
}

public enum NodeState
{
    Untested, Open, Closed
}


public enum MovementState
{
    Patrol, Return, Seek, MoveAttackPosition, Attack//UpAttack, DownAttack, LeftAttack, RightAttack
}

public enum PatrolDirection
{
    LeftDown, LeftUp, RightDown, RightUp, DownLeft, UpLeft, DownRight, UpRight
}