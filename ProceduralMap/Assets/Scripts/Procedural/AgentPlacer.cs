using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class AgentPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab, playerPrefab;
        [SerializeField] private int playerRoomIndex;
        [SerializeField] private CinemachineVirtualCamera vCamera;

        [SerializeField] private List<int> roomEnemiesCount;

        private DungeonData dungeonData;

        [SerializeField] private bool showGizmo;

        private void Awake()
        {
            dungeonData = FindObjectOfType<DungeonData>();
        }

        public void PlaceAgents()
        {
            if (dungeonData == null) return;

            roomEnemiesCount.Clear();
            roomEnemiesCount.Add(0);
            for (var i = 1; i < dungeonData.Rooms.Count; i++)
            {
                var ran = Random.Range(0, 10);
                roomEnemiesCount.Add(ran);
            }

            for (var i = 0; i < dungeonData.Rooms.Count; i++)
            {
                var room = dungeonData.Rooms[i];
                var roomGraph = new RoomGraph(room.FloorTiles);

                var roomFloor = new HashSet<Vector2Int>(room.FloorTiles);
                roomFloor.IntersectWith(dungeonData.Path);

                var roomMap = roomGraph.RunBfs(roomFloor.First(), room.PropPositions);

                room.PositionsAccessibleFromPath = roomMap.Keys.OrderBy(_ => Guid.NewGuid()).ToList();

                if (roomEnemiesCount.Count > i)
                {
                    PlaceEnemies(room, roomEnemiesCount[i]);
                }

                if (i != playerRoomIndex) continue;
                var player = Instantiate(playerPrefab);
                player.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f;
                vCamera.Follow = player.transform;
                vCamera.LookAt = player.transform;
                dungeonData.PlayerReference = player;
            }
        }

        private void PlaceEnemies(Room room, int enemiesCount)
        {
            for (var k = 0; k < enemiesCount; k++)
            {
                if (room.PositionsAccessibleFromPath.Count <= k) return;
                var enemy = Instantiate(enemyPrefab);
                enemy.transform.localPosition = room.PositionsAccessibleFromPath[k] + Vector2.one * 0.5f;
                room.EnemiesInTheRoom.Add(enemy);
            }
        }
    }

    public class RoomGraph
    {
        private static readonly List<Vector2Int> FourDirections = new()
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        private readonly Dictionary<Vector2Int, List<Vector2Int>> _graph = new();

        public RoomGraph(HashSet<Vector2Int> roomFloor)
        {
            foreach (var pos in roomFloor)
            {
                List<Vector2Int> neighbours = new();
                foreach (var direction in FourDirections)
                {
                    var newPos = pos + direction;
                    if (roomFloor.Contains(newPos)) neighbours.Add(newPos);
                }

                _graph.Add(pos, neighbours);
            }
        }

        public Dictionary<Vector2Int, Vector2Int> RunBfs(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
        {
            Queue<Vector2Int> nodesToVisit = new();
            nodesToVisit.Enqueue(startPos);

            HashSet<Vector2Int> visitedNodes = new() { startPos };

            Dictionary<Vector2Int, Vector2Int> map = new() { { startPos, startPos } };

            while (nodesToVisit.Count > 0)
            {
                var node = nodesToVisit.Dequeue();
                var neighbours = _graph[node];

                foreach (var neighbourPosition in neighbours)
                {
                    if (visitedNodes.Contains(neighbourPosition) == false &&
                        occupiedNodes.Contains(neighbourPosition) == false)
                    {
                        nodesToVisit.Enqueue(neighbourPosition);
                        visitedNodes.Add(neighbourPosition);
                        map[neighbourPosition] = node;
                    }
                }
            }

            return map;
        }
    }
}