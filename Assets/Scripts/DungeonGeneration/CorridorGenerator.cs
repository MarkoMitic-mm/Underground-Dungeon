using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Erstellt Korridore zwischen den Räumen des BSP-Baums.
/// Verbindet benachbarte Blattknoten miteinander.
/// </summary>
public class CorridorGenerator
{
    /// <summary>
    /// Generiert Korridore zwischen Räumen im BSP-Baum.
    /// </summary>
    /// <param name="rootNode">Der Wurzelknoten des BSP-Baums.</param>
    /// <param name="dungeonData">Die Dungeon-Datenstruktur zum Speichern der Korridore.</param>
    public void GenerateCorridors(BSPNode rootNode, DungeonData dungeonData)
    {
        GenerateCorridorsRecursive(rootNode, dungeonData);
    }

    /// <summary>
    /// Durchläuft den BSP-Baum rekursiv und erstellt Korridore zwischen Kinderknoten.
    /// </summary>
    private void GenerateCorridorsRecursive(BSPNode node, DungeonData dungeonData)
    {
        if (node.IsLeaf()) return;

        // Verbinde linken und rechten Kindknoten
        if (node.LeftChild != null && node.RightChild != null)
        {
            BSPNode leftNode = GetLeafNode(node.LeftChild);
            BSPNode rightNode = GetLeafNode(node.RightChild);

            if (leftNode?.Room.HasValue == true && rightNode?.Room.HasValue == true)
            {
                CreateCorridor(leftNode.Room.Value, rightNode.Room.Value, dungeonData);
            }
        }

        // Rekursiv für Kinder verarbeiten
        if (node.LeftChild != null) GenerateCorridorsRecursive(node.LeftChild, dungeonData);
        if (node.RightChild != null) GenerateCorridorsRecursive(node.RightChild, dungeonData);
    }

    /// <summary>
    /// Findet den "repräsentativen" Blattknoten eines Teilbaums (üblicherweise der erste Blatt).
    /// </summary>
    private BSPNode GetLeafNode(BSPNode node)
    {
        if (node.IsLeaf()) return node;
        return node.LeftChild != null ? GetLeafNode(node.LeftChild) : GetLeafNode(node.RightChild);
    }

    /// <summary>
    /// Erstellt einen L-förmigen Korridor zwischen zwei Räumen.
    /// </summary>
    private void CreateCorridor(RectInt room1, RectInt room2, DungeonData dungeonData)
    {

        Vector2Int start = Vector2Int.RoundToInt(room1.center);
        Vector2Int end = Vector2Int.RoundToInt(room2.center);

        // Horizontal dann Vertikal
        if (Random.value > 0.5f)
        {
            CreateHorizontalCorridor(start.x, end.x, start.y, dungeonData);
            CreateVerticalCorridor(start.y, end.y, end.x, dungeonData);
        }
        else
        {
            // Vertikal dann Horizontal
            CreateVerticalCorridor(start.y, end.y, start.x, dungeonData);
            CreateHorizontalCorridor(start.x, end.x, end.y, dungeonData);
        }
    }

    /// <summary>
    /// Erstellt einen horizontalen Korridorabschnitt.
    /// </summary>
    private void CreateHorizontalCorridor(int x1, int x2, int y, DungeonData dungeonData)
    {
        int minX = Mathf.Min(x1, x2);
        int maxX = Mathf.Max(x1, x2);

        for (int x = minX; x <= maxX; x++)
        {
            Vector2Int pos = new Vector2Int(x, y);
            if (!dungeonData.FloorTiles.Contains(pos))
            {
                dungeonData.CorridorTiles.Add(pos);
            }
        }
    }

    /// <summary>
    /// Erstellt einen vertikalen Korridorabschnitt.
    /// </summary>
    private void CreateVerticalCorridor(int y1, int y2, int x, DungeonData dungeonData)
    {
        int minY = Mathf.Min(y1, y2);
        int maxY = Mathf.Max(y1, y2);

        for (int y = minY; y <= maxY; y++)
        {
            Vector2Int pos = new Vector2Int(x, y);
            if (!dungeonData.FloorTiles.Contains(pos))
            {
                dungeonData.CorridorTiles.Add(pos);
            }
        }
    }
}
