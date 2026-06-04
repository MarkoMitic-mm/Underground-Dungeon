using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Size")]
    public int dungeonWidth = 100;
    public int dungeonHeight = 100;

    [Header("BSP Settings")]
    public int minRoomSize = 10;

    private BSPNode rootNode;
    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        BSPGenerator bspGenerator = new BSPGenerator();

        rootNode = bspGenerator.GenerateTree(
            new RectInt(0, 0, dungeonWidth, dungeonHeight),
            minRoomSize
        );

        Debug.Log($"Dungeon Area: {rootNode.Area}");
        
        Debug.Log("Root: " + rootNode.Area);

        Debug.Log("Left Child: " + rootNode.LeftChild.Area);

        Debug.Log("Right Child: " + rootNode.RightChild.Area);
    }
}
