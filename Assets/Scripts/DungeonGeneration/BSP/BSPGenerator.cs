using UnityEngine;
/// <summary>
/// Erstellt einen einfachen BSP-Baum.
/// Aktuell wird der Dungeon nur einmal vertikal
/// in zwei gleich große Bereiche geteilt.
/// </summary>
public class BSPGenerator
{
    private int _minNodeSize;

    /// <summary>
    /// Generiert einen BSP-Baum für den gegebenen Dungeon-Bereich und die minimale Raumgröße.
    /// 
    /// </summary>
    public BSPNode GenerateTree(RectInt dungeonArea, int minRoomSize)
    {
        _minNodeSize = minRoomSize;
        BSPNode root = new BSPNode(dungeonArea);
        TrySplit(root);
        return root;
    }

    /// <summary>
    /// Teilt den gegebenen Knoten rekursiv, bis die Bereiche kleiner als die minimale Raumgröße sind.
    /// </summary>
    /// <param name="node">Der Knoten, der geteilt werden soll.</param>
    private void TrySplit(BSPNode node)
    {
        if (!ShouldSplit(node)) return;

        bool splitHorizontal = ShouldSplitHorizontal(node.Area);

        if (splitHorizontal)
        {
            int splitY = Random.Range(
                node.Area.yMin + _minNodeSize,
                node.Area.yMax - _minNodeSize
            );
            node.LeftChild = new BSPNode(new RectInt(node.Area.xMin, node.Area.yMin, node.Area.width, splitY - node.Area.yMin));
            node.RightChild = new BSPNode(new RectInt(node.Area.xMin, splitY, node.Area.width, node.Area.yMax - splitY));
        }
        else
        {
            int splitX = Random.Range(
                node.Area.xMin + _minNodeSize,
                node.Area.xMax - _minNodeSize
            );
            node.LeftChild = new BSPNode(new RectInt(node.Area.xMin, node.Area.yMin, splitX - node.Area.xMin, node.Area.height));
            node.RightChild = new BSPNode(new RectInt(splitX, node.Area.yMin, node.Area.xMax - splitX, node.Area.height));
        }

        TrySplit(node.LeftChild);
        TrySplit(node.RightChild);
    }


    /// <summary>
    /// Bestimmt, ob der gegebene Knoten weiter geteilt werden sollte, basierend auf seiner Größe im Vergleich zur minimalen Raumgröße.
    /// </summary>
    /// <param name="node">Der Knoten, der überprüft werden soll.</param>
    /// <returns>True, wenn der Knoten weiter geteilt werden sollte, sonst False.</returns>
    private bool ShouldSplit(BSPNode node)
    {
        return node.Area.width > _minNodeSize * 2
            || node.Area.height > _minNodeSize * 2;
    }


    /// <summary>
    /// Bestimmt, ob der gegebene Bereich horizontal oder vertikal geteilt werden sollte, basierend auf seinem Seitenverhältnis.
    /// </summary>
    /// <param name="area">Der Bereich, der überprüft werden soll.</param>
    /// <returns>True, wenn der Bereich horizontal geteilt werden sollte, sonst False.</returns>
    private bool ShouldSplitHorizontal(RectInt area)
    {
        if (area.width > area.height * 1.25f) return false; // wide  split vertical
        if (area.height > area.width * 1.25f) return true;  // tall  split horizontal
        return Random.value > 0.5f;                         // square  coin flip
    }
}
