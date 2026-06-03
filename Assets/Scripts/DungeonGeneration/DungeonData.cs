using System.Collections.Generic;
using UnityEngine;

public class DungeonData
{
    public HashSet<Vector2Int> FloorTiles;
    public HashSet<Vector2Int> WallTiles;
    public List<Room> Rooms;
}
