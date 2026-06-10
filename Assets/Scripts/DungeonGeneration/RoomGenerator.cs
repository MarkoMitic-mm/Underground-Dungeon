using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration
{
    /// <summary>
    /// Erstellt Räume basierend auf den Blättern des BSP-Baums.
    /// Jeder Blattknoten repräsentiert einen potenziellen Raum.
    /// </summary>
    public class RoomGenerator
    {
        private int _padding;
        
        /// <summary>
        /// Erstellt einen RoomGenerator mit optionalem Padding.
        /// </summary>
        /// <param name="padding">Der Abstand zwischen Raum und Knotenrand.</param>
        public RoomGenerator(int padding = 1)
        {
            _padding = padding;
        }

        /// <summary>
        /// Generiert Räume für alle Blätter des BSP-Baums und aktualisiert die DungeonData mit den Rauminformationen.
        /// </summary>
        /// <param name="rootNode">Der Wurzelknoten des BSP-Baums.</param>
        /// <param name="dungeonData">Die Dungeon-Datenstruktur, die aktualisiert werden soll.</param>
        /// <returns>Eine Liste der generierten Räume.</returns>
        public List<Room> GenerateRooms(BSPNode rootNode, DungeonData dungeonData)
        {
            List<Room> rooms = new List<Room>();
            GenerateRoomsRecursive(rootNode, rooms, dungeonData);
            return rooms;
        }

        /// <summary>
        /// Durchläuft den BSP-Baum rekursiv, um Räume in den Blättern zu erstellen und die DungeonData zu aktualisieren.
        /// </summary>
        /// <param name="node">Der aktuelle Knoten im BSP-Baum.</param>
        /// <param name="rooms">Die Liste der bisher generierten Räume.</param>
        /// <param name="dungeonData">Die Dungeon-Datenstruktur, die aktualisiert werden soll.</param>
        private void GenerateRoomsRecursive(BSPNode node, List<Room> rooms, DungeonData dungeonData)
        {
            if (node.IsLeaf())
            {
                Room room = CarveRoom(node);
                node.Room = room.Bounds;
                rooms.Add(room);

                // fill floor tiles and TileToRoom lookup
                foreach (var pos in GetRoomTiles(room))
                {
                    dungeonData.FloorTiles.Add(pos);
                    dungeonData.TileToRoom[pos] = room;
                }
            }
            else
            {
                if (node.LeftChild != null) GenerateRoomsRecursive(node.LeftChild, rooms, dungeonData);
                if (node.RightChild != null) GenerateRoomsRecursive(node.RightChild, rooms, dungeonData);
            }
        }

        /// <summary>
        /// "Carvt" einen Raum innerhalb der gegebenen Partition des BSP-Knotens, 
        /// wobei die Größe und Position zufällig gewählt werden, 
        /// aber innerhalb der Grenzen des Knotens und unter Berücksichtigung des Paddings liegen.
        /// </summary>
        /// <param name="node">Der BSP-Knoten, innerhalb dessen ein Raum erstellt werden soll.</param>
        /// <returns>Der erstellte Raum.</returns>
        private Room CarveRoom(BSPNode node)
        {
            RectInt area = node.Area;

            // Zufällige Größe des Raums, mindestens halb so groß wie die Partition, aber mit genügend Platz für Padding
            int roomWidth = Random.Range(area.width / 2, area.width - _padding * 2);
            int roomHeight = Random.Range(area.height / 2, area.height - _padding * 2);

            // Zufällige Position innerhalb der Partition
            int roomX = Random.Range(area.xMin + _padding, area.xMax - roomWidth - _padding);
            int roomY = Random.Range(area.yMin + _padding, area.yMax - roomHeight - _padding);

            return new Room
            {
                Bounds = new RectInt(roomX, roomY, roomWidth, roomHeight)
            };
        }

        /// <summary>
        /// Gibt alle Positionen zurück, die zu einem Raum gehören, basierend auf den Grenzen des Raums.
        /// </summary>
        /// <param name="room">Der Raum, dessen Positionen zurückgegeben werden sollen.</param>
        /// <returns>Eine Aufzählung aller Positionen innerhalb des Raums.</returns>
        private IEnumerable<Vector2Int> GetRoomTiles(Room room)
        {
            for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
                for (int y = room.Bounds.yMin; y < room.Bounds.yMax; y++)
                    yield return new Vector2Int(x, y);
        }
    }
}
