using UnityEngine;

public class BSPGenerator
{
    public BSPNode GenerateTree(RectInt dungeonArea, int minRoomSize)
    {
        BSPNode root = new BSPNode(dungeonArea);

        int splitX = dungeonArea.width / 2;

        root.LeftChild = new BSPNode(
            new RectInt(
                dungeonArea.x,
                dungeonArea.y,
                splitX,
                dungeonArea.height));

        root.RightChild = new BSPNode(
            new RectInt(
                dungeonArea.x + splitX,
                dungeonArea.y,
                dungeonArea.width - splitX,
                dungeonArea.height));

        return root;
    }
}
