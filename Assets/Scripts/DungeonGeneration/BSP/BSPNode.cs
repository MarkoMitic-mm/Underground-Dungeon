using UnityEngine;

public class BSPNode
{
    public RectInt Area;

    public BSPNode LeftChild;
    public BSPNode RightChild;

    public BSPNode(RectInt area)
    {
        Area = area;
    }

    public bool IsLeaf()
    {
        return LeftChild == null && RightChild == null;
    }
}
