using UnityEngine;
/// <summary>
/// Erstellt einen einfachen BSP-Baum.
/// Aktuell wird der Dungeon nur einmal vertikal
/// in zwei gleich groﬂe Bereiche geteilt.
/// </summary>
public class BSPGenerator
{
    /// <summary>
    /// Erzeugt die Wurzel des BSP-Baums und teilt
    /// den Dungeon in einen linken und rechten Bereich.
    /// </summary>
    public BSPNode GenerateTree(RectInt dungeonArea, int minRoomSize)
    {
        BSPNode root = new BSPNode(dungeonArea);

        // Berechnet die Mitte des Dungeons auf der X-Achse.
        int splitX = dungeonArea.width / 2;

        //Linker Teilbereich
        root.LeftChild = new BSPNode(
            new RectInt(
                dungeonArea.x,
                dungeonArea.y,
                splitX,
                dungeonArea.height));

        //Rechter Teilbereich
        root.RightChild = new BSPNode(
            new RectInt(
                dungeonArea.x + splitX,
                dungeonArea.y,
                dungeonArea.width - splitX,
                dungeonArea.height));

        return root;
    }
}
