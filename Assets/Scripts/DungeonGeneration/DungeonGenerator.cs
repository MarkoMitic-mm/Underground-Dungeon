using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGeneration
{
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

        [Header("Visualizer Settings")]
        // Verzögerung zwischen den Schritten der Visualisierung (falls benötigt).
        public float stepDelay = 0.5f;

        [Header("Game Settings")]
        // Referenz zum Spielerobjekt, das im Dungeon platziert werden soll.
        public GameObject player;

        [Header("Corridor Settings")]
        // Breite der Korridore, die zwischen den Räumen erzeugt werden.
        public int corridorWidth = 2;

        // Speichert alle generierten Daten des Dungeons, einschließlich der BSP-Struktur, Räume und Korridore.
        private DungeonData _dungeonData;

        // Generator für Korridore
        private CorridorGenerator _corridorGenerator;

        // Generator für Wände
        private WallGenerator _wallGenerator;

        // Visualizer für die Tilemaps
        private TilemapVisualizer _tilemapVisualizer;

        /// <summary>
        /// Initialisiert die Dungeon-Generierung und startet den Visualisierungsprozess.
        /// </summary>
        void Start()
        {
            // TilemapVisualizer aus der Szene laden
            _tilemapVisualizer = GetComponent<TilemapVisualizer>();
            if (_tilemapVisualizer == null)
            {
                Debug.LogError("TilemapVisualizer nicht gefunden! Bitte als Komponente hinzufügen.");
                return;
            }

            StartCoroutine(GenerateDungeonStepByStep());
        }

        /// <summary>
        /// Erstellt den BSP-Baum für den gesamten Dungeonbereich.
        /// </summary>
        IEnumerator GenerateDungeonStepByStep()
        {
            if (player != null)
                player.SetActive(false);

            // Initialisiert die Dungeon-Datenstruktur, um alle Informationen über den Dungeon zu speichern.
            _dungeonData = DungeonManager.Instance.DungeonData;
            yield return new WaitForSeconds(stepDelay);

            // Zeichnet jeden Raum innerhalb des BSP-Baums und speichert die Raumdaten.
            var floorPositions = new List<Vector2Int>();
            foreach (var room in _dungeonData.Rooms)
            {
                for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
                    for (int y = room.Bounds.yMin; y < room.Bounds.yMax; y++)
                        floorPositions.Add(new Vector2Int(x, y));
            }
            _tilemapVisualizer.PaintFloorTiles(floorPositions);
            yield return new WaitForSeconds(stepDelay);

            // Korridore werden visualisiert
            _tilemapVisualizer.PaintFloorTiles(_dungeonData.CorridorTiles);
            yield return new WaitForSeconds(stepDelay);

            // Wände auf der Tilemap visualisieren
            if (_dungeonData.WallTiles != null && _dungeonData.WallTiles.Count > 0)
            {
                _tilemapVisualizer.PaintTiles(_tilemapVisualizer.floorTilemap, _tilemapVisualizer.wallTile, _dungeonData.WallTiles);
            }

            // Spieler an der Spawn-Position platzieren
            if (player != null)
            {
                player.transform.position = new Vector3(
                    _dungeonData.SpawnPoint.x,
                    _dungeonData.SpawnPoint.y,
                    0
                );
                player.SetActive(true);
            }
        }

        /// <summary>
        /// Zeichnet den Dungeon im Editor.
        /// </summary>
        void OnDrawGizmos()
        {
            if (_dungeonData?.RootNode == null) return;
            DrawNode(_dungeonData.RootNode);

            // Korridore stabil aus DungeonData zeichnen (kein erneutes Erzeugen)
            if (_dungeonData.CorridorTiles != null && _dungeonData.CorridorTiles.Count > 0)
            {
                Gizmos.color = Color.red;
                foreach (var p in _dungeonData.CorridorTiles)
                {
                    Vector3 center = new Vector3(p.x + 0.5f, p.y + 0.5f, 0);
                    Gizmos.DrawCube(center, Vector3.one * 0.9f);
                }
            }
        }

        /// <summary>
        /// Zeichnet den Bereich des aktuellen Knotens und seiner Kinder.
        /// Blätter werden grün, innere Knoten weiß dargestellt.
        /// </summary>
        /// <param name="node"></param>
        void DrawNode(BSPNode node)
        {
            //Zeichnet den Bereich des aktuellen Knotens. Blätter werden grün, innere Knoten weiß dargestellt.
            Gizmos.color = node.IsLeaf() ? Color.green : Color.white;
            Gizmos.DrawWireCube(
                new Vector3(node.Area.center.x, node.Area.center.y, 0),
                new Vector3(node.Area.width, node.Area.height, 0)
            );

            //Zeichnet den Raum innerhalb des Knotens, falls vorhanden, in Blau.
            if (node.Room.HasValue)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(
                    new Vector3(node.Room.Value.center.x, node.Room.Value.center.y, 0),
                    new Vector3(node.Room.Value.width, node.Room.Value.height, 0)
                );
            }

            //Rekursiv die Kinderknoten zeichnen, um die gesamte Struktur sichtbar zu machen.
            if (node.LeftChild != null) DrawNode(node.LeftChild);
            if (node.RightChild != null) DrawNode(node.RightChild);
        }
    }
}
