using UnityEngine;

/// <summary>
/// Einstiegspunkt der Dungeon-Generierung.
/// Erstellt beim Start einen BSP-Baum, der später
/// zur Raum- und Korridorerzeugung verwendet wird.
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Size")]
    // Gesamtgröße des Dungeons in Zellen/Tiles.
    public int dungeonWidth = 100;
    public int dungeonHeight = 100;

    [Header("BSP Settings")]
    // Kleinste erlaubte Raumgröße.
    public int minRoomSize = 10;

    // Speichert alle generierten Daten des Dungeons, einschließlich der BSP-Struktur, Räume und Korridore.
    private DungeonData _dungeonData;

    void Start()
    {
        GenerateDungeon();
    }

    /// <summary>
    /// Erstellt den BSP-Baum für den gesamten Dungeonbereich.
    /// </summary>
    void GenerateDungeon()
    {
        // Initialisiert die Dungeon-Datenstruktur, um alle Informationen über den Dungeon zu speichern.
        _dungeonData = new DungeonData();

        BSPGenerator bspGenerator = new BSPGenerator();

        // Starte die BSP-Aufteilung für den kompletten Dungeon.
        _dungeonData.RootNode = bspGenerator.GenerateTree(
            new RectInt(0, 0, dungeonWidth, dungeonHeight),
            minRoomSize
        );

        // Erstellt Räume basierend auf den Blättern des BSP-Baums. Jeder Blattknoten repräsentiert einen potenziellen Raum.
        RoomGenerator roomGenerator = new RoomGenerator();
        _dungeonData.Rooms = roomGenerator.GenerateRooms(
            _dungeonData.RootNode, _dungeonData
        );

        // Debug-Ausgaben zur Kontrolle der ersten Aufteilung.
        Debug.Log($"Dungeon Area: {_dungeonData.RootNode.Area}");
        Debug.Log("Root: " + _dungeonData.RootNode.Area);
        Debug.Log("Left Child: " + _dungeonData.RootNode.LeftChild.Area);
        Debug.Log("Right Child: " + _dungeonData.RootNode.RightChild.Area);
        Debug.Log($"Generated {_dungeonData.Rooms.Count} rooms");
    }
    /// <summary>
    /// Nächster Schritt: Rekursive Aufteilung der Bereiche, um weitere Unterbereiche zu erstellen.
    /// Das bestehende erweitern und sichtbar machen, wie die Aufteilung weitergeht.
    /// Weitere Methoden zur Raum- und Korridorerzeugung würden hier folgen.
    /// </summary> 


    /// <summary>
    /// Zeichnet den Dungeon im Editor.
    /// </summary>
    void OnDrawGizmos()
    {
        if (_dungeonData?.RootNode == null) return;
        DrawNode(_dungeonData.RootNode);
    }

    /// <summary>
    /// Zeichnet den Bereich des aktuellen Knotens und seiner Kinder.
    /// Blätter werden grün, innere Knoten weiß dargestellt.
    /// </summary>
    /// <param name="node"></param>
    void DrawNode(BSPNode node)
    {
        //Zeichnet den Bereich des aktuellen Knotens. Blätter werden grün, innere Knoten weiß dargestellt.
        Gizmos.color = node.IsLeaf() ? Color.green : Color.white;
        Gizmos.DrawWireCube(
            new Vector3(node.Area.center.x, node.Area.center.y, 0),
            new Vector3(node.Area.width, node.Area.height, 0)
        );

        //Zeichnet den Raum innerhalb des Knotens, falls vorhanden, in Blau.
        if (node.Room.HasValue)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(
                new Vector3(node.Room.Value.center.x, node.Room.Value.center.y, 0),
                new Vector3(node.Room.Value.width, node.Room.Value.height, 0)
            );
        }

            //Rekursiv die Kinderknoten zeichnen, um die gesamte Struktur sichtbar zu machen.
            if (node.LeftChild != null) DrawNode(node.LeftChild);
        if (node.RightChild != null) DrawNode(node.RightChild);
    }
}
