using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    public Tilemap floorTilemap;
    public TileBase floorTile;
    public TileBase wallTile;  // Neu hinzufügen

    // Einfache Komfortmethode, die im restlichen Code verwendet wird
    public void PaintFloorTiles(IEnumerable<Vector2Int> positions)
    {
        PaintTiles(floorTilemap, floorTile, positions);
    }

    // Malt eine Sammlung von Positionen auf die übergebene Tilemap mit einem einzelnen TileBase.
    // Verwendet standardmäßig ein gebündeltes SetTiles-Aufruf für bessere Performance.
    public void PaintTiles(Tilemap tilemap, TileBase tile, IEnumerable<Vector2Int> positions, bool useBatch = true, Vector3Int offset = default)
    {
        if (tilemap == null || tile == null || positions == null) return;

        if (useBatch)
        {
            // Konvertiere Positionen und bereite ein gleichgroßes TileBase-Array für SetTiles vor
            var posArray = positions.Select(p => new Vector3Int(p.x + offset.x, p.y + offset.y, offset.z)).ToArray();
            if (posArray.Length == 0) return;
            var tiles = Enumerable.Repeat(tile, posArray.Length).ToArray();
            tilemap.SetTiles(posArray, tiles);
        }
        else
        {
            foreach (var p in positions)
            {
                var v = new Vector3Int(p.x + offset.x, p.y + offset.y, offset.z);
                tilemap.SetTile(v, tile);
            }
        }
    }

    // Löscht eine Menge von Tiles aus einer Tilemap (einzeln).
    public void ClearTiles(Tilemap tilemap, IEnumerable<Vector2Int> positions, Vector3Int offset = default)
    {
        if (tilemap == null || positions == null) return;
        foreach (var p in positions)
        {
            var v = new Vector3Int(p.x + offset.x, p.y + offset.y, offset.z);
            tilemap.SetTile(v, null);
        }
    }

    // Löscht alle Tiles auf der übergebenen Tilemap.
    public void ClearAllTiles(Tilemap tilemap)
    {
        if (tilemap == null) return;
        tilemap.ClearAllTiles();
    }
}
