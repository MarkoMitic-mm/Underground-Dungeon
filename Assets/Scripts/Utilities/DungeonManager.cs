using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Singleton-Instanz des DungeonManagers, um globalen Zugriff zu ermöglichen.
    public static DungeonManager Instance { get; private set; }
    // Speichert alle generierten Daten des Dungeons, einschließlich der BSP-Struktur, Räume und Korridore.
    public DungeonData DungeonData { get; private set; }

    /// <summary>
    /// Initialisiert die Singleton-Instanz des DungeonManagers und stellt sicher, dass nur eine Instanz existiert.
    /// </summary>
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Generiert den Dungeon basierend auf den angegebenen Parametern für Breite, Höhe und minimale Raumgröße.
    /// </summary>
    /// <param name="width">Die Breite des Dungeons in Zellen/Tiles.</param>
    /// <param name="height">Die Höhe des Dungeons in Zellen/Tiles.</param>
    /// <param name="minRoomSize">Die kleinste erlaubte Raumgröße.</param>
    public void GenerateDungeon(int width, int height, int minRoomSize)
    {
        // Initialisiert die Dungeon-Datenstruktur, um alle Informationen über den Dungeon zu speichern.
        DungeonData = new DungeonData();

        // Starte die BSP-Aufteilung für den kompletten Dungeon.
        BSPGenerator bspGenerator = new BSPGenerator();
        DungeonData.RootNode = bspGenerator.GenerateTree(
            new RectInt(0, 0, width, height),
            minRoomSize
        );
        Debug.Log("Step 1: BSP tree built");

        // Erstellt Räume basierend auf den Blättern des BSP-Baums. Jeder Blattknoten repräsentiert einen potenziellen Raum.
        RoomGenerator roomGenerator = new RoomGenerator();
        DungeonData.Rooms = roomGenerator.GenerateRooms(DungeonData.RootNode, DungeonData);
        DungeonData.SpawnPoint = DungeonData.Rooms[0].Center;

        Debug.Log($"Spawn point: {DungeonData.SpawnPoint}");
        Debug.Log($"First room center: {DungeonData.Rooms[0].Center}");

        Debug.Log("Step 2: Rooms generated");
        Debug.Log($"Step 2: {DungeonData.Rooms.Count} rooms generated");

        // CorridorGenerator initialisieren und Korridore einmalig erzeugen (ausgelagert)
        CorridorGenerator corridorGenerator = new CorridorGenerator();
        corridorGenerator.GenerateCorridors(DungeonData.RootNode, DungeonData);
        Debug.Log("Step 3: Corridors generated");

        // WallGenerator initialisieren und Wände generieren
        WallGenerator wallGenerator = new WallGenerator();
        DungeonData.WallTiles = wallGenerator.CreateWalls(DungeonData);
        Debug.Log("Step 4: Walls generated");

        // Debug-Ausgaben zur Kontrolle der ersten Aufteilung.
        Debug.Log($"Dungeon Area: {DungeonData.RootNode.Area}");
        Debug.Log("Root: " + DungeonData.RootNode.Area);
        Debug.Log("Left Child: " + DungeonData.RootNode.LeftChild.Area);
        Debug.Log("Right Child: " + DungeonData.RootNode.RightChild.Area);
        Debug.Log($"Generated {DungeonData.Rooms.Count} rooms");
        Debug.Log($"Generated {DungeonData.WallTiles.Count} walls");
    }
}
