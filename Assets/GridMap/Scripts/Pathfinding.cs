using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;


    private Grid<PathNode> grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;


    public Pathfinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero,
                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 targetWorldPoistion)
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(targetWorldPoistion, out int targetX, out int targetY);

        List<PathNode> path = FindPath(startX, startY, targetX, targetY);

        if (path == null)
        {
            return null;
        }

        List<Vector3> vectorPath = new List<Vector3>();

        foreach (PathNode pathNode in path)
        {
            vectorPath.Add(grid.GetWorldPosition(pathNode.x, pathNode.y) +
                     new Vector3(grid.GetCellSize(), grid.GetCellSize()) * .5f);
        }

        return vectorPath;
    }


    public List<PathNode> FindPath(int startX, int startY, int targetX, int targetY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode targetNode = grid.GetGridObject(targetX, targetY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();


        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }


        startNode.gCost = 0;
        startNode.hCost = CalulateDistanceCost(startNode, targetNode);
        startNode.CalculateFCost();


        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == targetNode)
            {
                return CalculatePath(targetNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalulateDistanceCost(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalulateDistanceCost(neighbourNode, targetNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) // Left
        {
            neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y));

            if (currentNode.y - 1 >= 0) // Down Left
            {
                neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y - 1));
            }
            if (currentNode.y + 1 < grid.GetHeight()) // Up Left
            {
                neighbourList.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y + 1));
            }
        }

        if (currentNode.x + 1 < grid.GetWidth()) // Right
        {
            neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y));

            if (currentNode.y - 1 >= 0) // Down Right
            {
                neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y - 1));
            }
            if (currentNode.y + 1 < grid.GetHeight()) // Up Right
            {
                neighbourList.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y + 1));
            }
        }

        if (currentNode.y - 1 >= 0) // Down
        {
            neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y - 1));
        }

        if (currentNode.y + 1 < grid.GetHeight()) // Up
        {
            neighbourList.Add(grid.GetGridObject(currentNode.x, currentNode.y + 1));
        }


        return neighbourList;
    }

    public PathNode GetPathNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode targetNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(targetNode);
        PathNode currentNode = targetNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();


        return path;
    }


    private int CalulateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }


    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}
