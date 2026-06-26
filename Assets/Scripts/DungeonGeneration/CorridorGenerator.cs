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

        if (node.LeftChild != null) GenerateCorridorsRecursive(node.LeftChild, dungeonData);
        if (node.RightChild != null) GenerateCorridorsRecursive(node.RightChild, dungeonData);

        // get all leaves from each side
        List<BSPNode> leftLeaves = GetAllLeaves(node.LeftChild);
        List<BSPNode> rightLeaves = GetAllLeaves(node.RightChild);

        // find the closest pair between the two sides
        BSPNode bestLeft = null;
        BSPNode bestRight = null;
        float bestDist = float.MaxValue;

        foreach (BSPNode left in leftLeaves)
        {
            if (!left.Room.HasValue) continue;
            foreach (BSPNode right in rightLeaves)
            {
                if (!right.Room.HasValue) continue;
                float dist = Vector2Int.Distance(
                    Vector2Int.RoundToInt(left.Room.Value.center),
                    Vector2Int.RoundToInt(right.Room.Value.center)
                );
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestLeft = left;
                    bestRight = right;
                }
            }
        }

        if (bestLeft != null && bestRight != null)
            CreateCorridor(bestLeft.Room.Value, bestRight.Room.Value, dungeonData);
    }

    private List<BSPNode> GetAllLeaves(BSPNode node)
    {
        List<BSPNode> leaves = new List<BSPNode>();
        CollectLeaves(node, leaves);
        return leaves;
    }

    private void CollectLeaves(BSPNode node, List<BSPNode> leaves)
    {
        if (node == null) return;
        if (node.IsLeaf()) { leaves.Add(node); return; }
        CollectLeaves(node.LeftChild, leaves);
        CollectLeaves(node.RightChild, leaves);
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
