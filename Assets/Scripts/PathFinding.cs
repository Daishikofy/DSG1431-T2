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
    private MovementState State;

    private Point BaseLeftPos;
    private Point currentPos;
    private Point currentRealPos;
    private Point playerPos;
    private Point playerRealPos;

    private Point PatrolStartPosition;
    private Point PatrolWalkDistance;
    private PatrolStartDirection StartDirection;

    private int tamGridX;
    private int tamGridY;

    private IList<IList<Node>> NodesGrid;
    private IList<Node> Path;

    private System.Random Rand;

    public PathFinding(int attackRange, int seekRange, Point patrolWalkDistance, Point patrolStartPosition, Point baseLeft, Point topRight, PatrolStartDirection startDirection)
    {
        AttackRange = attackRange;
        SeekRange = seekRange;
        State = MovementState.Patrol;

        PatrolStartPosition = patrolStartPosition;
        PatrolWalkDistance = patrolWalkDistance;
        StartDirection = startDirection;

        BaseLeftPos = baseLeft;
        tamGridX = Math.Abs(baseLeft.X - topRight.X) + 1;
        tamGridY = Math.Abs(baseLeft.Y - topRight.Y) + 1;
        
        Rand = new System.Random();
    }

    public Point DefineDirection(DynamicGrid grid, Point currentPos, Point playerPos)
    {
        this.currentRealPos = currentPos;
        this.playerRealPos = playerPos;
        this.currentPos = new Point(currentPos.X - BaseLeftPos.X, currentPos.Y - BaseLeftPos.Y);
        this.playerPos = new Point(playerPos.X - BaseLeftPos.X, playerPos.Y - BaseLeftPos.Y);

        SetState();

        if (State == MovementState.Attack)
            return new Point(0,0);
        else if (State == MovementState.Patrol)
            return PatrolMovement();
        else
        {
            InstantiateVariables(grid);
            Point direction;
            if (State == MovementState.MoveAttackPosition)
            {
                direction = MoveToAttackPosition();
                if (!PointIsZero(direction))
                    return direction;

                State = MovementState.Seek;
            }

            Point destination;
            if (State == MovementState.Seek)
                destination = playerPos;
            else if (State == MovementState.Return)
                destination = PatrolStartPosition;


            direction = TwoWayMovement(currentPos, destination);
            if (PointIsZero(direction))
                direction = AStar();

            return direction;
        }

    }

    private void InstantiateVariables(DynamicGrid grid)
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
                    G = Math.Abs(currentPos.X - i) + Math.Abs(currentPos.Y - j),
                    H = Math.Abs(this.playerPos.X - i) + Math.Abs(this.playerPos.Y - j),
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
        var distanceX = Math.Abs(currentPos.X - playerPos.X);
        var distanceY = Math.Abs(currentPos.Y - playerPos.Y);

        if (distanceX > SeekRange || distanceY > SeekRange)
            State = MovementState.Patrol;
        else if (distanceX > AttackRange || distanceY > AttackRange)
            State = MovementState.Seek;
        else if (distanceX > 0 && distanceY > 0)
            State = MovementState.MoveAttackPosition;
        else
            State = MovementState.Attack;
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

        Point direction = StraightLineMovement(startPos, firstDestination1, moveInXFirst);
        if (!PointIsZero(direction))
        {
            if (!PointIsZero(StraightLineMovement(firstDestination1, destination, !moveInXFirst)))
                return direction;
        }

        direction = StraightLineMovement(startPos, firstDestination2, !moveInXFirst);
        if (!PointIsZero(direction))
        {
            if (!PointIsZero(StraightLineMovement(firstDestination2, destination, moveInXFirst)))
                return direction;
        }

        return new Point(0, 0);
    }

    private Point StraightLineMovement(Point startPos, Point destination, bool moveInX)
    {
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
            if (moveInX)
                isWalkable = NodesGrid[curPosInAxys][startPos.Y].IsWalkable;
            else
                isWalkable = NodesGrid[startPos.X][curPosInAxys].IsWalkable;

            if (!isWalkable)
                return new Point(0,0);
        }

        if (moveInX)
            return new Point(movement, 0);

        return new Point(0, movement);
    }

    private bool PointIsZero(Point a)
    {
        if (a.X == 0 && a.Y == 0)
            return true;
        return false;
    }

    #region A Star

    public Point AStar()
    {        
        Node currentNode = NodesGrid[currentPos.X][currentPos.Y];
        if (Search(currentNode))
        {
            Point nextTile = Path.FirstOrDefault().Location;
            var direction = new Point(nextTile.X - currentPos.X, nextTile.Y - currentPos.Y);
            return direction;
        }
        else
            return new Point(0, 0);
    }
       
    private void GeraPath(Node finalNode)
    {
        Path = new List<Node>();
        Node node = finalNode;

        while(node.Location.X != currentPos.X && node.Location.Y != currentPos.Y)
        {
            Path.Add(node);
            node = node.ParentNode;
        }

        Path = Path.Reverse().ToList();
    }

    private bool Search(Node currentNode)
    {
        currentNode.State = NodeState.Closed;
        List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
        nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
        foreach (var nextNode in nextNodes)
        {
            if (nextNode.Location == playerPos)
            {
                GeraPath(nextNode);
                return true;
            }
            else
            {
                if (Search(nextNode)) // Note: Recurses back into Search(Node)
                    return true;
            }
        }
        return false;
    }

    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        IList<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in nextLocations)
        {
            int x = location.X;
            int y = location.Y;

            Node node = NodesGrid[x][y];
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
        if (locationX - 1 > 0)
            adjacentNodes.Add(NodesGrid[locationX - 1][locationY].Location);
        if (locationY + 1 < tamGridY)
            adjacentNodes.Add(NodesGrid[locationX][locationY + 1].Location);
        if (locationY - 1 > tamGridY)
            adjacentNodes.Add(NodesGrid[locationX][locationY - 1].Location);


        return adjacentNodes;
    }
    #endregion

    #region Move to Attack Position

    public Point MoveToAttackPosition()
    {
        bool moveEixoX = Rand.Next() % 2 == 0;
        Point destination, secondaryDestination;

        if (moveEixoX)
        {
            destination = new Point(currentPos.X - playerPos.X, currentPos.Y);
            secondaryDestination = new Point(currentPos.X, currentPos.Y - playerPos.Y);
        }
        else
        {
            destination = new Point(currentPos.X, currentPos.Y - playerPos.Y);
            secondaryDestination = new Point(currentPos.X - playerPos.X, currentPos.Y);
        }

        Point direction = StraightLineMovement(currentPos, destination, moveEixoX);
        if (PointIsZero(direction))
            direction = StraightLineMovement(currentPos, secondaryDestination, !moveEixoX);

        return direction;
    }

    #endregion

    #region Patrol Movement
    public Point PatrolMovement()
    {
        return new Point(0, 0);
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

public enum PatrolStartDirection
{
    LeftDown, LeftUp, RightDown, RightUp, DownLeft, UpLeft, DownRight, UpRight
}