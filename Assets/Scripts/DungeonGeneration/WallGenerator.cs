using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generiert Wände um Räume und Korridore herum.
/// Wände werden an allen Positionen platziert, die direkt neben Boden-Tiles liegen.
/// </summary>
public class WallGenerator
{
    /// <summary>
    /// Erstellt Wände basierend auf den Boden- und Korridortiles.
    /// Wände werden an Positionen platziert, die neben Boden-Tiles liegen, aber selbst kein Boden sind.
    /// </summary>
    /// <param name="dungeonData">Die Dungeon-Datenstruktur mit Raum- und Korridorinformationen.</param>
    /// <returns>Ein HashSet aller Wandpositionen.</returns>
    public HashSet<Vector2Int> CreateWalls(DungeonData dungeonData)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        HashSet<Vector2Int> allFloorTiles = new HashSet<Vector2Int>(dungeonData.FloorTiles);
        allFloorTiles.UnionWith(dungeonData.CorridorTiles);

        // Durchlaufe alle Boden-Tiles und prüfe deren Nachbarn
        foreach (var floorTile in allFloorTiles)
        {
            // Prüfe alle 4 Nachbarpositionen (oben, unten, links, rechts)
            Vector2Int[] neighbors = new[]
            {
                floorTile + Vector2Int.up,
                floorTile + Vector2Int.down,
                floorTile + Vector2Int.left,
                floorTile + Vector2Int.right
            };

            foreach (var neighbor in neighbors)
            {
                // Wenn Nachbar kein Boden ist, wird er eine Wand
                if (!allFloorTiles.Contains(neighbor))
                {
                    walls.Add(neighbor);
                }
            }
        }

        // Entferne Wände, die außerhalb des Dungeon-Bereichs liegen (optional)
        walls.RemoveWhere(w => IsOutOfBounds(w, dungeonData));

        return walls;
    }

    /// <summary>
    /// Prüft, ob eine Position außerhalb des Dungeon-Bereichs liegt.
    /// </summary>
    private bool IsOutOfBounds(Vector2Int pos, DungeonData dungeonData)
    {
        RectInt area = dungeonData.RootNode.Area;
        return pos.x < area.xMin || pos.x >= area.xMax || pos.y < area.yMin || pos.y >= area.yMax;
    }
}
