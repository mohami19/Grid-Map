﻿
using UnityEngine;
using CodeMonkey.Utils;
using System.Collections.Generic;

public class PathfindingTesting : MonoBehaviour
{

    private Pathfinding pathfinding;
    private void Start()
    {
        pathfinding = new Pathfinding(10, 10);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f,
                                   new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f,
                                   Color.green, 5f);
                }
            }

        }
    }
}

