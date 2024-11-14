using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Procedural
{
    public class RoomDataExtractor : MonoBehaviour
    {
        private DungeonData _dungeonData;

        [SerializeField] private bool showGizmo;

        public UnityEvent onFinishedRoomProcessing;

        private void Awake()
        {
            _dungeonData = FindObjectOfType<DungeonData>();
        }

        public void ProcessRooms()
        {
            if (_dungeonData == null) return;

            foreach (var room in _dungeonData.Rooms)
            {
                foreach (var tilePosition in room.FloorTiles)
                {
                    var neighboursCount = 4;

                    if (room.FloorTiles.Contains(tilePosition + Vector2Int.up) == false)
                    {
                        room.NearWallTilesUp.Add(tilePosition);
                        neighboursCount--;
                    }

                    if (room.FloorTiles.Contains(tilePosition + Vector2Int.down) == false)
                    {
                        room.NearWallTilesDown.Add(tilePosition);
                        neighboursCount--;
                    }

                    if (room.FloorTiles.Contains(tilePosition + Vector2Int.right) == false)
                    {
                        room.NearWallTilesRight.Add(tilePosition);
                        neighboursCount--;
                    }

                    if (room.FloorTiles.Contains(tilePosition + Vector2Int.left) == false)
                    {
                        room.NearWallTilesLeft.Add(tilePosition);
                        neighboursCount--;
                    }

                    if (neighboursCount <= 2) room.CornerTiles.Add(tilePosition);
                    if (neighboursCount == 4) room.InnerTiles.Add(tilePosition);
                }

                room.NearWallTilesUp.ExceptWith(room.CornerTiles);
                room.NearWallTilesDown.ExceptWith(room.CornerTiles);
                room.NearWallTilesLeft.ExceptWith(room.CornerTiles);
                room.NearWallTilesRight.ExceptWith(room.CornerTiles);
            }

            Invoke(nameof(RunEvent), 1);
        }

        public void RunEvent()
        {
            onFinishedRoomProcessing?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            if (_dungeonData == null || showGizmo == false) return;

            foreach (var room in _dungeonData.Rooms)
            {
                Gizmos.color = Color.yellow;
                foreach (var floorPosition in room.InnerTiles)
                {
                    if (_dungeonData.Path.Contains(floorPosition)) continue;
                    Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
                }

                Gizmos.color = Color.blue;
                foreach (var floorPosition in room.NearWallTilesUp.Where(floorPosition => !_dungeonData.Path.Contains(floorPosition)))
                {
                    Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
                }

                Gizmos.color = Color.green;
                foreach (var floorPosition in room.NearWallTilesDown.Where(floorPosition => !_dungeonData.Path.Contains(floorPosition)))
                {
                    Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
                }

                Gizmos.color = Color.white;
                foreach (var floorPosition in room.NearWallTilesRight.Where(floorPosition => !_dungeonData.Path.Contains(floorPosition)))
                {
                    Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
                }

                Gizmos.color = Color.cyan;
                foreach (var floorPosition in room.NearWallTilesLeft.Where(floorPosition => !_dungeonData.Path.Contains(floorPosition)))
                {
                    Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
                }

                Gizmos.color = Color.magenta;
                foreach (var floorPosition in room.CornerTiles.Where(floorPosition => !_dungeonData.Path.Contains(floorPosition)))
                {
                    Gizmos.DrawCube(floorPosition + Vector2.one * 0.5f, Vector2.one);
                }
            }
        }
    }
}