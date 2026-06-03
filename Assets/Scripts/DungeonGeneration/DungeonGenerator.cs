using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int dungeonWidth;
    public int dungeonHeight;

    private RoomGenerator roomGenerator;
    private CorridorGenerator corridorGenerator;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Räume erzeugen
        // Korridore erzeugen
        // Dungeon aufbauen
    }
}
