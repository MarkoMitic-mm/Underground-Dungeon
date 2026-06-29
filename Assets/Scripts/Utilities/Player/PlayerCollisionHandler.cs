using UnityEngine;

/// <summary>
/// Verwaltet die Kollisionserkennung und Bewegungsvalidierung des Spielers.
/// Verhindert, dass der Spieler über Wände, Korridore und Boden hinaus laufen kann.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    // Größe des Spielers als Radius
    public float playerRadius = 0.35f;

    private Rigidbody2D _rb;
    private DungeonData _dungeonData;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dungeonData = DungeonManager.Instance.DungeonData;

        // Spieler-Renderer für korrekte Sortierung einrichten
        SetupSpriteRendering();
    }

    void FixedUpdate()
    {
        // Prüfe die aktuelle Position des Spielers und passe die Sortierung an
        UpdatePlayerSorting();
    }

    /// <summary>
    /// Überprüft, ob eine Position für den Spieler begehbar ist.
    /// Verwendet den Spieler-Radius zur präzisen Kollisionserkennung.
    /// </summary>
    public bool IsPositionWalkable(Vector3 position)
    {
        Vector2Int tilePos = Vector2Int.FloorToInt(new Vector2(position.x, position.y));
        return _dungeonData.FloorTiles.Contains(tilePos) || 
               _dungeonData.CorridorTiles.Contains(tilePos);
    }

    /// <summary>
    /// Überprüft, ob eine Bewegung zum Punkt zulässig ist.
    /// Prüft mehrere Punkte basierend auf dem Spieler-Radius.
    /// </summary>
    public bool CanMoveTo(Vector3 targetPosition)
    {
        // Prüfe das Zentrum und die Ecken basierend auf playerRadius
        float radius = playerRadius;
        
        Vector3[] checkPoints = new[]
        {
            targetPosition,
            targetPosition + new Vector3(radius, 0, 0),
            targetPosition + new Vector3(-radius, 0, 0),
            targetPosition + new Vector3(0, radius, 0),
            targetPosition + new Vector3(0, -radius, 0)
        };

        // Alle Punkte müssen begehbar sein
        foreach (var point in checkPoints)
        {
            if (!IsPositionWalkable(point))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Richtet die Sprite-Sortierung des Spielers basierend auf seiner Y-Position ein.
    /// Dies verhindert, dass der Spieler unter Korridoren verschwindet.
    /// </summary>
    private void SetupSpriteRendering()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 0;
        }
    }

    /// <summary>
    /// Aktualisiert die Sortierungsreihenfolge des Spielers basierend auf seiner Y-Position.
    /// Sprites mit höherer Y-Position werden unter Sprites mit niedrigerer Y-Position gezeichnet.
    /// </summary>
    private void UpdatePlayerSorting()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-_rb.position.y * 100);
        }
    }
}
