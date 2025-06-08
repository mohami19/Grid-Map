using System;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.VisualScripting;

public class Grid
{

    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridValueChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize) + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }

        gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
        if (OnGridValueChanged != null) OnGridValueChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public void AddValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }

    public int GetValue(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return 0;
        }

        return gridArray[x, y];
    }

    public int GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    public void AddValue(Vector3 worldPostion, int value, int fullValueRange, int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

        GetXY(worldPostion, out int originX, out int OriginY);
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                int addValueAmount = value;
                if (radius > fullValueRange)
                {
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }

                AddValue(originX + x, OriginY + y, addValueAmount);

                if (x != 0) AddValue(originX - x, OriginY + y, addValueAmount);

                if (y != 0) AddValue(originX + x, OriginY - y, addValueAmount);

                if (x != 0 && y != 0) AddValue(originX - x, OriginY - y, addValueAmount);
            }
        }
    }
}