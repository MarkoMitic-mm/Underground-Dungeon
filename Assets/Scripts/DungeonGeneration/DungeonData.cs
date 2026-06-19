using System.Collections.Generic;
using UnityEngine;

public class DungeonData
{
    public BSPNode RootNode { get; set; }
    public List<Room> Rooms { get; set; } = new List<Room>();
    public HashSet<Vector2Int> FloorTiles { get; set; } = new HashSet<Vector2Int>();
    public Dictionary<Vector2Int, Room> TileToRoom { get; set; } = new Dictionary<Vector2Int, Room>();
    public HashSet<Vector2Int> CorridorTiles { get; set; } = new HashSet<Vector2Int>();
}
