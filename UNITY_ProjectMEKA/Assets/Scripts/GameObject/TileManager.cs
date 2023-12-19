using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class TileManager
{
    public List<(Tile, Vector3Int)> lowTiles = new List<(Tile, Vector3Int)>();
    public List<(Tile, Vector3Int)> highTiles = new List<(Tile, Vector3Int)>();
    public List<(Tile, Vector3Int)> allTiles = new List<(Tile, Vector3Int)>();

    public TileManager()
    {
        var allLowTiles = GameObject.FindGameObjectsWithTag(Tags.lowTile);
        var allHighTiles = GameObject.FindGameObjectsWithTag(Tags.highTile);
        foreach(var tile in allLowTiles)
        {
            SetTiles(tile);
        }
        foreach(var tile in allHighTiles)
        {
            SetTiles(tile);
        }
    }

    public void SetTiles(GameObject tile)
    {
        var tileController = tile.GetComponentInChildren<Tile>();
        var index = GetTileIndex(tileController);
        tileController.index = index;
        if (tile.tag == Tags.lowTile)
        {
            lowTiles.Add((tileController, index));
        }
        else
        {
            highTiles.Add((tileController, index));
        }
        allTiles.Add((tileController, index));
    }

    public Vector3Int GetTileIndex(Tile tile)
    {
        LayerMask layerMask = 1 << tile.gameObject.layer;
        RaycastHit hit;
        if(Physics.Raycast(tile.transform.parent.position + Vector3.up * 10f, Vector3.down, out hit))
        {
            return Utils.Vector3ToVector3Int(hit.point);
        }
        return new Vector3Int(-100,-100,-100);
    }
}
