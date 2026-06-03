using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    public Tilemap floorTilemap;
    public TileBase floorTile;

    public void PaintFloorTiles(
        IEnumerable<Vector2Int> positions)
    {
        foreach (var pos in positions)
        {
            floorTilemap.SetTile(
                (Vector3Int)pos,
                floorTile);
        }
    }
}
