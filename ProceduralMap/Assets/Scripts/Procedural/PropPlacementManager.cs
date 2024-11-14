using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Procedural
{
    public class PropPlacementManager : MonoBehaviour
    {
        private DungeonData dungeonData;

        [SerializeField] private List<Prop> propsToPlace;
        [SerializeField, Range(0, 1)] private float cornerPropPlacementChance = 0.7f;

        [SerializeField] private GameObject propPrefab;
        public UnityEvent onFinished;

        private void Awake()
        {
            dungeonData = FindObjectOfType<DungeonData>();
        }

        public void ProcessRooms()
        {
            if (dungeonData == null) return;
            foreach (var room in dungeonData.Rooms)
            {
                var cornerProps = propsToPlace.Where(x => x.corner).ToList();
                PlaceCornerProps(room, cornerProps);

                var leftWallProps = propsToPlace
                    .Where(x => x.nearWallLeft)
                    .OrderByDescending(x => x.propSize.x * x.propSize.y)
                    .ToList();
                PlaceProps(room, leftWallProps, room.NearWallTilesLeft, PlacementOriginCorner.BottomLeft);

                var rightWallProps = propsToPlace
                    .Where(x => x.nearWallRight)
                    .OrderByDescending(x => x.propSize.x * x.propSize.y)
                    .ToList();
                PlaceProps(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.TopRight);

                var topWallProps = propsToPlace
                    .Where(x => x.nearWallUp)
                    .OrderByDescending(x => x.propSize.x * x.propSize.y)
                    .ToList();
                PlaceProps(room, topWallProps, room.NearWallTilesUp, PlacementOriginCorner.TopLeft);

                var downWallProps = propsToPlace
                    .Where(x => x.nearWallDown)
                    .OrderByDescending(x => x.propSize.x * x.propSize.y)
                    .ToList();
                PlaceProps(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.BottomLeft);

                var innerProps = propsToPlace
                    .Where(x => x.inner)
                    .OrderByDescending(x => x.propSize.x * x.propSize.y)
                    .ToList();
                PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);
            }

            Invoke(nameof(RunEvent), 1);
        }

        public void RunEvent()
        {
            onFinished?.Invoke();
        }

        private IEnumerator TutorialCoroutine(Action code)
        {
            yield return new WaitForSeconds(3);
            code();
        }

        private void PlaceCornerProps(Room room, IReadOnlyList<Prop> cornerProps)
        {
            var tempChance = cornerPropPlacementChance;

            foreach (var cornerTile in room.CornerTiles)
            {
                if (UnityEngine.Random.value < cornerPropPlacementChance)
                {
                    var propToPlace = cornerProps[UnityEngine.Random.Range(0, cornerProps.Count)];
                    PlacePropGameObjectAt(room, cornerTile, propToPlace);
                    if (propToPlace.placeAsGroup) PlaceGroupObject(room, cornerTile, propToPlace, 2);
                }
                else
                {
                    tempChance = Mathf.Clamp01(tempChance + 0.1f);
                }
            }
        }


        private void PlaceGroupObject(Room room, Vector2Int groupOriginPosition, Prop propToPlace, int searchOffset)
        {
            var count = UnityEngine.Random.Range(propToPlace.groupMinCount, propToPlace.groupMaxCount) - 1;
            count = Mathf.Clamp(count, 0, 8);

            List<Vector2Int> availableSpaces = new();
            for (var xOffset = -searchOffset; xOffset <= searchOffset; xOffset++)
            {
                for (var yOffset = -searchOffset; yOffset <= searchOffset; yOffset++)
                {
                    var tempPos = groupOriginPosition + new Vector2Int(xOffset, yOffset);
                    if (room.FloorTiles.Contains(tempPos) && !dungeonData.Path.Contains(tempPos) &&
                        !room.PropPositions.Contains(tempPos))
                    {
                        availableSpaces.Add(tempPos);
                    }
                }
            }

            availableSpaces.OrderBy(_ => Guid.NewGuid());

            var tempCount = count < availableSpaces.Count ? count : availableSpaces.Count;
            for (var i = 0; i < tempCount; i++)
            {
                PlacePropGameObjectAt(room, availableSpaces[i], propToPlace);
            }
        }

        private void PlacePropGameObjectAt(Room room, Vector2Int placementPosition, Prop propToPlace)
        {
            var prop = Instantiate(propPrefab);
            var propSpriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();

            propSpriteRenderer.sprite = propToPlace.propSprite;

            var capsuleCollider2D = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
        

            capsuleCollider2D.offset = new Vector2(0, -0.1f);
            capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;
            var size = new Vector2(propToPlace.propSize.x * 0.6f, propToPlace.propSize.y * 0.5f);
            capsuleCollider2D.size = size;

            prop.transform.localPosition = (Vector2)placementPosition;
            propSpriteRenderer.transform.localPosition = (Vector2)propToPlace.propSize * 0.5f;

            room.PropPositions.Add(placementPosition);
            room.PropObjectReferences.Add(prop);
        }

        private void PlaceProps(Room room, List<Prop> wallProps, HashSet<Vector2Int> availableTiles,
            PlacementOriginCorner placement)
        {
            var tempPositions = new HashSet<Vector2Int>(availableTiles);
            tempPositions.ExceptWith(dungeonData.Path);

            foreach (var propToPlace in wallProps)
            {
                var quantity =
                    UnityEngine.Random.Range(propToPlace.placementQuantityMin, propToPlace.placementQuantityMax + 1);

                for (var i = 0; i < quantity; i++)
                {
                    tempPositions.ExceptWith(room.PropPositions);
                    var availablePositions = tempPositions.OrderBy(_ => Guid.NewGuid()).ToList();
                    if (TryPlacingPropBruteForce(room, propToPlace, availablePositions, placement) == false) break;
                }
            }
        }

        private bool TryPlacingPropBruteForce(Room room, Prop propToPlace, List<Vector2Int> availablePositions,
            PlacementOriginCorner placement)
        {
            for (var i = availablePositions.Count - 1; i >= 0; i--)
            {
                var position = availablePositions[i];
                if (room.PropPositions.Contains(position)) continue;

                var freePositionsAround = TryToFitProp(propToPlace, availablePositions, position, placement);

                if (freePositionsAround.Count != propToPlace.propSize.x * propToPlace.propSize.y) continue;
                PlacePropGameObjectAt(room, position, propToPlace);
                foreach (var pos in freePositionsAround)
                {
                    room.PropPositions.Add(pos);
                }

                if (propToPlace.placeAsGroup) PlaceGroupObject(room, position, propToPlace, 1);
                return true;
            }

            return false;
        }

        private static List<Vector2Int> TryToFitProp(Prop prop, ICollection<Vector2Int> availablePositions, Vector2Int originPosition,
            PlacementOriginCorner placement)
        {
            List<Vector2Int> freePositions = new();
            switch (placement)
            {
                case PlacementOriginCorner.BottomLeft:
                {
                    for (var xOffset = 0; xOffset < prop.propSize.x; xOffset++)
                    {
                        for (var yOffset = 0; yOffset < prop.propSize.y; yOffset++)
                        {
                            var tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                            if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                        }
                    }

                    break;
                }
                case PlacementOriginCorner.BottomRight:
                {
                    for (var xOffset = -prop.propSize.x + 1; xOffset <= 0; xOffset++)
                    {
                        for (var yOffset = 0; yOffset < prop.propSize.y; yOffset++)
                        {
                            var tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                            if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                        }
                    }

                    break;
                }
                case PlacementOriginCorner.TopLeft:
                {
                    for (var xOffset = 0; xOffset < prop.propSize.x; xOffset++)
                    {
                        for (var yOffset = -prop.propSize.y + 1; yOffset <= 0; yOffset++)
                        {
                            var tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                            if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                        }
                    }

                    break;
                }
                default:
                {
                    for (var xOffset = -prop.propSize.x + 1; xOffset <= 0; xOffset++)
                    {
                        for (var yOffset = -prop.propSize.y + 1; yOffset <= 0; yOffset++)
                        {
                            var tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                            if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                        }
                    }

                    break;
                }
            }

            return freePositions;
        }
    }

    public enum PlacementOriginCorner
    {
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight
    }
}