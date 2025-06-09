﻿
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class Testing : MonoBehaviour
{
    [SerializeField] private TilemapVisual tilemapVisual;

    private Tilemap.TilemapObject.TilemapSprite tilemapSprite;

    private Tilemap tilemap;

    private void Start()
    {
        tilemap = new Tilemap(20, 10, 10f, Vector3.zero);
        tilemap.SetTilemapVisual(tilemapVisual);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);
            // Debug.Log($"Clicked on tile at: {x}, {y}");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Path;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Dirt;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            tilemap.Save();
            CMDebug.TextPopupMouse("Tilemap saved");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            tilemap.Load();
            CMDebug.TextPopupMouse("Tilemap loaded");
        }

    }
}

