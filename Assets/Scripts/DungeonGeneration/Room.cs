using UnityEngine;

public class Room
{
    public RectInt Bounds;

    public Vector2Int Center
    {
        get
        {
            return new Vector2Int(
                Bounds.x + Bounds.width / 2,
                Bounds.y + Bounds.height / 2);
        }
    }
}
