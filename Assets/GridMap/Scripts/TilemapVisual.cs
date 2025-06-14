using System;
using System.Collections.Generic;
using UnityEngine;

public class TilemapVisual : MonoBehaviour
{

    [Serializable]
    public struct TilemapSpriteUV
    {
        public Tilemap.TilemapObject.TilemapSprite tilemapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVcoords
    {
        public Vector2 uv00;
        public Vector2 uv11;

        public UVcoords(Vector2 uv00, Vector2 uv11)
        {
            this.uv00 = uv00;
            this.uv11 = uv11;
        }
    }

    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUVs;

    private Grid<Tilemap.TilemapObject> grid;
    private Mesh mesh;

    private bool updateMesh = false;

    private Dictionary<Tilemap.TilemapObject.TilemapSprite, UVcoords> uvCoordsDictionary;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary = new Dictionary<Tilemap.TilemapObject.TilemapSprite, UVcoords>();

        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVs)
        {
            uvCoordsDictionary[tilemapSpriteUV.tilemapSprite] = new UVcoords(
                uv00: new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth,
                            tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11: new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth,
                            tilemapSpriteUV.uv11Pixels.y / textureHeight)
            );
        }
    }


    public void SetGrid(Tilemap tilemap, Grid<Tilemap.TilemapObject> grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
        tilemap.OnLoaded += Tilemap_OnLoaded;
    }

    private void Tilemap_OnLoaded(object sender, EventArgs e)
    {
        updateMesh = true;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<Tilemap.TilemapObject>.OnGridObjectChangedEventArgs e)
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

                Tilemap.TilemapObject gridObject = grid.GetGridObject(x, y);
                Tilemap.TilemapObject.TilemapSprite tilemapSprit = gridObject.GetTilemapSprite();

                Vector2 gridCellUV00, gridCellUV11;
                if (tilemapSprit == Tilemap.TilemapObject.TilemapSprite.None)
                {
                    gridCellUV00 = Vector2.zero;
                    gridCellUV11 = Vector2.zero;
                    baseSize = Vector3.zero;
                }
                else
                {
                    UVcoords uvCoords = uvCoordsDictionary[tilemapSprit];
                    gridCellUV00 = uvCoords.uv00;
                    gridCellUV11 = uvCoords.uv11;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index,
                            grid.GetWorldPosition(x, y) + baseSize * .5f, 0f, baseSize, gridCellUV00, gridCellUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
