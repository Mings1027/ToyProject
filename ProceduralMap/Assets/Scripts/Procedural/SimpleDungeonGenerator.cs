using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class SimpleDungeonGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private int corridorSize;

        [SerializeField] private Tilemap roomMap, colliderMap;
        [SerializeField] private TileBase roomFloorTile, wallTile, pathFloorTile;

        [SerializeField] private InputActionReference generate;

        [SerializeField] private int roomCount;

        public UnityEvent onFinishedRoomGeneration;

        public List<Vector2Int> roomCenterList = new();

        private static readonly List<Vector2Int> FourDirections = new()
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        private DungeonData dungeonData;

        private void Awake()
        {
            dungeonData = FindObjectOfType<DungeonData>();
            if (dungeonData == null) dungeonData = gameObject.AddComponent<DungeonData>();
            generate.action.performed += Generate;
        }

        private void Generate(InputAction.CallbackContext obj)
        {
            roomMap.ClearAllTiles();
            colliderMap.ClearAllTiles();
            dungeonData.Reset();

            roomCenterList.Clear();

            var z = Vector2Int.zero;
            roomCenterList.Add(z);
            dungeonData.Rooms.Add(GenerateRectangularRoomAt(z * corridorSize, roomSize));
            
            for (var i = 0; i < roomCount; i++)
            {
                var ranDir = FourDirections[Random.Range(0, 4)];
            
                dungeonData.Path.UnionWith(CreateStraightCorridor(z * corridorSize, (z + ranDir) * corridorSize));
                z += ranDir;
                roomCenterList.Add(z);
                dungeonData.Rooms.Add(GenerateRectangularRoomAt(z * corridorSize, roomSize));
            }

            GenerateDungeonCollider();

            //When you use this invoke you have to use method "GenerateRectangularRoomAt" and "CreateStraightCorridor"
            // onFinishedRoomGeneration?.Invoke();
        }

        private void GenerateDungeonCollider()
        {
            HashSet<Vector2Int> dungeonTiles = new();
            foreach (var room in dungeonData.Rooms)
            {
                dungeonTiles.UnionWith(room.FloorTiles);
            }

            dungeonTiles.UnionWith(dungeonData.Path);

            HashSet<Vector2Int> colliderTiles = new();
            foreach (var tilePosition in dungeonTiles)
            {
                foreach (var direction in FourDirections)
                {
                    var newPosition = tilePosition + direction;
                    if (dungeonTiles.Contains(newPosition) == false)
                    {
                        colliderTiles.Add(newPosition);
                    }
                }
            }

            foreach (var pos in colliderTiles)
            {
                colliderMap.SetTile((Vector3Int)pos, wallTile);
            }

            colliderMap.GetComponent<CompositeCollider2D>().offset = new Vector2(0, -0.2f);
        }

        private Room GenerateRectangularRoomAt(Vector2 roomCenterPosition, Vector2Int roomsSize)
        {
            var half = roomsSize / 2;
            HashSet<Vector2Int> roomTiles = new();
            for (var x = -half.x; x < half.x; x++)
            {
                for (var y = -half.y; y < half.y; y++)
                {
                    var position = roomCenterPosition + new Vector2(x, y);
                    var positionInt = roomMap.WorldToCell(position);
                    roomTiles.Add((Vector2Int)positionInt);
                    roomMap.SetTile(positionInt, roomFloorTile);
                }
            }

            return new Room(roomCenterPosition, roomTiles);
        }

        private IEnumerable<Vector2Int> CreateStraightCorridor(Vector2Int startPosition, Vector2Int endPosition)
        {
            HashSet<Vector2Int> corridorTiles = new() { startPosition };
            roomMap.SetTile((Vector3Int)startPosition, pathFloorTile);
            corridorTiles.Add(endPosition);
            roomMap.SetTile((Vector3Int)endPosition, pathFloorTile);

            var direction = Vector2Int.CeilToInt(((Vector2)endPosition - startPosition).normalized);
            var currentPosition = startPosition;

            // while (Vector2.Distance(currentPosition, endPosition) > 1)
            for (var i = 0; i < corridorSize; i++)
            {
                currentPosition += direction;
                corridorTiles.Add(currentPosition);
                roomMap.SetTile((Vector3Int)currentPosition, pathFloorTile);
            }

            return corridorTiles;
        }
    }
}