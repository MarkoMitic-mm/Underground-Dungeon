using System.Collections.Generic;
using UnityEngine;

public class DungeonData
{
    public HashSet<Vector2Int> FloorTiles = new();
    public HashSet<Vector2Int> WallTiles = new();
    public HashSet<Vector2Int> CorridorTiles =new();
    public List<Room> Rooms = new();
    public Dictionary<Vector2Int, Room> TileToRoom = new();

    // Startpunkt für den Spieler, z.B. die Mitte des ersten Raums.
    public Vector2Int SpawnPoint;

    // Wurzelknoten des BSP-Baums.
    public BSPNode RootNode;
}
