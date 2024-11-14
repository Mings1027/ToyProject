using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class DungeonData : MonoBehaviour
    {
        public List<Room> Rooms { get; private set; } = new();
        public HashSet<Vector2Int> Path { get; private set; } = new();

        public GameObject PlayerReference { get; set; }

        public void Reset()
        {
            foreach (var room in Rooms)
            {
                foreach (var item in room.PropObjectReferences)
                {
                    Destroy(item);
                }

                foreach (var item in room.EnemiesInTheRoom)
                {
                    Destroy(item);
                }
            }

            Rooms = new List<Room>();
            Path = new HashSet<Vector2Int>();
            Destroy(PlayerReference);
        }

        public IEnumerator TutorialCoroutine(Action code)
        {
            yield return new WaitForSeconds(1);
            code();
        }
    }

    public class Room
    {
        public Vector2 RoomCenterPos { get; }
        public HashSet<Vector2Int> FloorTiles { get; }
        public HashSet<Vector2Int> NearWallTilesUp { get; } = new();
        public HashSet<Vector2Int> NearWallTilesDown { get; } = new();
        public HashSet<Vector2Int> NearWallTilesLeft { get; } = new();
        public HashSet<Vector2Int> NearWallTilesRight { get; } = new();
        public HashSet<Vector2Int> CornerTiles { get; } = new();
        public HashSet<Vector2Int> InnerTiles { get; } = new();

        public HashSet<Vector2Int> PropPositions { get; } = new();
        public List<GameObject> PropObjectReferences { get; } = new();

        public List<Vector2Int> PositionsAccessibleFromPath { get; set; } = new();
        public List<GameObject> EnemiesInTheRoom { get; } = new();

        public Room(Vector2 roomCenterPos, HashSet<Vector2Int> floorTiles)
        {
            RoomCenterPos = roomCenterPos;
            FloorTiles = floorTiles;
        }
    }
}