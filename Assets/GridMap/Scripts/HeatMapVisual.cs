using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    // private Grid grid;
    // private Mesh mesh;

    // private bool updateMesh = false;

    // private void Awake()
    // {
    //     mesh = new Mesh();
    //     GetComponent<MeshFilter>().mesh = mesh;
    // }


    // public void SetGrid(Grid grid)
    // {
    //     this.grid = grid;
    //     UpdateHeatMapVisual();

    //     grid.OnGridValueChanged += Grid_OnGridValueChanged;
    // }

    // private void Grid_OnGridValueChanged(object sender, Grid.OnGridObjectChangedEventArgs e)
    // {
    //     updateMesh = true;
    // }

    // private void LateUpdate()
    // {
    //     if (updateMesh)
    //     {
    //         updateMesh = false;
    //         UpdateHeatMapVisual();
    //     }
    // }

    // private void UpdateHeatMapVisual()
    // {

    //     MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices,
    //             out Vector2[] uv, out int[] triangles);

    //     for (int x = 0; x < grid.GetWidth(); x++)
    //     {
    //         for (int y = 0; y < grid.GetHeight(); y++)
    //         {
    //             int index = x * grid.GetHeight() + y;
    //             Vector3 baseSize = new Vector3(1, 1) * grid.GetCellSize();
    //             int gridValue = grid.GetValue(x, y);
    //             int maxGridValue = 100;
    //             float gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
    //             Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);
    //             MeshUtils.AddToMeshArrays(vertices, uv, triangles, index,
    //                         grid.GetWorldPosition(x, y) + baseSize * .5f, 0f, baseSize, gridCellUV, gridCellUV);
    //         }
    //     }

    //     mesh.vertices = vertices;
    //     mesh.uv = uv;
    //     mesh.triangles = triangles;
    // }
}
