using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration
{
    public class CorridorGenerator
    {
        // Erzeugt einen L-förmigen Korridor zwischen start und end auf einem Ganzzahl-Gitter.
        // Gibt eine Liste von Gitterkoordinaten zurück, die den Korridorpfad bilden (inklusive Endpunkte).
        // Der Korridor ist 1-Kachel breit. Die Biegerichtung wird zufällig gewählt, um Variation zu erzeugen.
        public List<Vector2Int> CreateCorridor(
            Vector2Int start,
            Vector2Int end)
        {
            var corridor = new List<Vector2Int>();

            // Zufällig wählen, ob horizontal->vertical oder vertical->horizontal gelaufen wird
            bool horizontalFirst = Random.value > 0.5f;

            Vector2Int current = start;
            corridor.Add(current);

            if (horizontalFirst)
            {
                // Horizontal bewegen, bis X übereinstimmt
                int stepX = end.x > current.x ? 1 : -1;
                while (current.x != end.x)
                {
                    current.x += stepX;
                    corridor.Add(current);
                }

                // Dann vertikal zum Ziel Y bewegen
                int stepY = end.y > current.y ? 1 : -1;
                while (current.y != end.y)
                {
                    current.y += stepY;
                    corridor.Add(current);
                }
            }
            else
            {
                // Zuerst vertikal
                int stepY = end.y > current.y ? 1 : -1;
                while (current.y != end.y)
                {
                    current.y += stepY;
                    corridor.Add(current);
                }

                int stepX = end.x > current.x ? 1 : -1;
                while (current.x != end.x)
                {
                    current.x += stepX;
                    corridor.Add(current);
                }
            }

            return corridor;
        }

        // Public helper: Erzeuge einmalig alle Korridore für den BSP-Baum und fülle DungeonData
        public void GenerateCorridors(BSPNode rootNode, DungeonData dungeonData)
        {
            if (rootNode == null || dungeonData == null) return;
            dungeonData.CorridorTiles.Clear();
            GenerateCorridorsRecursive(rootNode, dungeonData);
        }

        private void GenerateCorridorsRecursive(BSPNode node, DungeonData dungeonData)
        {
            if (node == null) return;

            // Suche in beiden Teilbäumen jeweils eine Raum-Mitte (falls vorhanden)
            Vector2Int? leftCenter = FindRoomCenter(node.LeftChild);
            Vector2Int? rightCenter = FindRoomCenter(node.RightChild);

            if (leftCenter.HasValue && rightCenter.HasValue)
            {
                var corridor = CreateCorridor(leftCenter.Value, rightCenter.Value);
                foreach (var p in corridor)
                {
                    dungeonData.CorridorTiles.Add(p);
                    // Optional: Korridore auch als Boden markieren
                    dungeonData.FloorTiles.Add(p);
                }
            }

            if (node.LeftChild != null) GenerateCorridorsRecursive(node.LeftChild, dungeonData);
            if (node.RightChild != null) GenerateCorridorsRecursive(node.RightChild, dungeonData);
        }

        // Findet rekursiv die nächste Room-Mitte im übergebenen Subtree.
        // Gibt null zurück, falls kein Room im Subtree existiert.
        private Vector2Int? FindRoomCenter(BSPNode node)
        {
            if (node == null) return null;
            if (node.Room.HasValue)
            {
                return Vector2Int.RoundToInt(node.Room.Value.center);
            }

            // Suche bevorzugt in der linken Seite, dann rechts (wahlfrei)
            Vector2Int? left = FindRoomCenter(node.LeftChild);
            if (left.HasValue) return left;
            return FindRoomCenter(node.RightChild);
        }
    }
}
