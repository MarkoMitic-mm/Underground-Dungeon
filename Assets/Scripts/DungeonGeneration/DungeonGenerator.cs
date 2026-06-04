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

    // Wurzelknoten des BSP-Baums.
    private BSPNode rootNode;
    void Start()
    {
        GenerateDungeon();
    }

    /// <summary>
    /// Erstellt den BSP-Baum für den gesamten Dungeonbereich.
    /// </summary>
    void GenerateDungeon()
    {
        BSPGenerator bspGenerator = new BSPGenerator();

        // Starte die BSP-Aufteilung für den kompletten Dungeon.
        rootNode = bspGenerator.GenerateTree(
            new RectInt(0, 0, dungeonWidth, dungeonHeight),
            minRoomSize
        );

        // Debug-Ausgaben zur Kontrolle der ersten Aufteilung.
        Debug.Log($"Dungeon Area: {rootNode.Area}");
        Debug.Log("Root: " + rootNode.Area);
        Debug.Log("Left Child: " + rootNode.LeftChild.Area);
        Debug.Log("Right Child: " + rootNode.RightChild.Area);
    }
    /// <summary>
    /// Nächster Schritt: Rekursive Aufteilung der Bereiche, um weitere Unterbereiche zu erstellen.
    /// Das bestehende erweitern und sichtbar machen, wie die Aufteilung weitergeht.
    /// Weitere Methoden zur Raum- und Korridorerzeugung würden hier folgen.
    /// </summary>
}
