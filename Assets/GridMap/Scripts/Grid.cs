using UnityEngine;
using System;
public class Grid
{

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }


    private int width;
    private int height;
    private Vector3 originPosition;
    private float cellSize;
    private int[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];

        Debug.Log(width + "x" + height + " grid created.");

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize) + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
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

        gridArray[x, y] = value;
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
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
}