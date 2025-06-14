using UnityEngine;

public class HeatMapGenericVisual : MonoBehaviour
{
    private Grid<HeatMapGridObject> grid;
    private Mesh mesh;

    private bool updateMesh = false;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }


    public void SetGrid(Grid<HeatMapGridObject> grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<HeatMapGridObject>.OnGridObjectChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual()
    {

        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices,
                out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();

                HeatMapGridObject gridObject = grid.GetGridObject(x, y);
                float gridValueNormalized = gridObject.GetValueNormalized();
                Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index,
                            grid.GetWorldPosition(x, y) + baseSize * .5f, 0f, baseSize, gridCellUV, gridCellUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}


public class HeatMapGridObject
{
    public const int MAX = 100;
    public const int MIN = 0;

    private Grid<HeatMapGridObject> grid;
    int x;
    int y;
    public int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}