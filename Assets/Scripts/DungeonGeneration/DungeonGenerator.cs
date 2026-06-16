using UnityEngine;

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

        // Speichert alle generierten Daten des Dungeons, einschließlich der BSP-Struktur, Räume und Korridore.
        private DungeonData _dungeonData;

        // Generator für Korridore
        private CorridorGenerator _corridorGenerator;

        // Visualizer für die Tilemaps
        private TilemapVisualizer _tilemapVisualizer;

        void Start()
        {
            // TilemapVisualizer aus der Szene laden
            _tilemapVisualizer = GetComponent<TilemapVisualizer>();
            if (_tilemapVisualizer == null)
            {
                Debug.LogError("TilemapVisualizer nicht gefunden! Bitte als Komponente hinzufügen.");
                return;
            }

            GenerateDungeon();
        }

        /// <summary>
        /// Erstellt den BSP-Baum für den gesamten Dungeonbereich.
        /// </summary>
        void GenerateDungeon()
        {
            // Initialisiert die Dungeon-Datenstruktur, um alle Informationen über den Dungeon zu speichern.
            _dungeonData = new DungeonData();

            BSPGenerator bspGenerator = new BSPGenerator();

            // Starte die BSP-Aufteilung für den kompletten Dungeon.
            _dungeonData.RootNode = bspGenerator.GenerateTree(
                new RectInt(0, 0, dungeonWidth, dungeonHeight),
                minRoomSize
            );

            // Erstellt Räume basierend auf den Blättern des BSP-Baums. Jeder Blattknoten repräsentiert einen potenziellen Raum.
            RoomGenerator roomGenerator = new RoomGenerator();
            _dungeonData.Rooms = roomGenerator.GenerateRooms(
                _dungeonData.RootNode, _dungeonData
            );

            // CorridorGenerator initialisieren und Korridore einmalig erzeugen (ausgelagert)
            _corridorGenerator = new CorridorGenerator();
            _corridorGenerator.GenerateCorridors(_dungeonData.RootNode, _dungeonData);

            // Visualisierung: Räume und Korridore auf die Tilemap zeichnen
            VisualizeDungeon();

            // Debug-Ausgaben zur Kontrolle der ersten Aufteilung.
            Debug.Log($"Dungeon Area: {_dungeonData.RootNode.Area}");
            Debug.Log("Root: " + _dungeonData.RootNode.Area);
            Debug.Log("Left Child: " + _dungeonData.RootNode.LeftChild.Area);
            Debug.Log("Right Child: " + _dungeonData.RootNode.RightChild.Area);
            Debug.Log($"Generated {_dungeonData.Rooms.Count} rooms");
        }

        /// <summary>
        /// Visualisiert den generierten Dungeon auf den Tilemaps.
        /// </summary>
        void VisualizeDungeon()
        {
            if (_tilemapVisualizer == null) return;

            // Boden-Tiles für alle Räume zeichnen
            var floorPositions = new System.Collections.Generic.List<Vector2Int>();
            foreach (var room in _dungeonData.Rooms)
            {
                for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
                {
                    for (int y = room.Bounds.yMin; y < room.Bounds.yMax; y++)
                    {
                        floorPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
            _tilemapVisualizer.PaintFloorTiles(floorPositions);

            // Korridore zeichnen (falls vorhanden)
            if (_dungeonData.CorridorTiles != null && _dungeonData.CorridorTiles.Count > 0)
            {
                _tilemapVisualizer.PaintFloorTiles(_dungeonData.CorridorTiles);
            }

            Debug.Log($"Visualisiert {floorPositions.Count} Boden-Tiles und {_dungeonData.CorridorTiles?.Count ?? 0} Korridor-Tiles");
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
