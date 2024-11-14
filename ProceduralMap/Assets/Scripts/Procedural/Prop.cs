using UnityEngine;

namespace Procedural
{
    [CreateAssetMenu]
    public class Prop : ScriptableObject
    {
        [Header("Prop data:")] public Sprite propSprite;
        public Vector2Int propSize = Vector2Int.one;

        [Space, Header("Placement type:")] 
        public bool corner = true;
        public bool nearWallUp = true;
        public bool nearWallDown = true;
        public bool nearWallRight = true;
        public bool nearWallLeft = true;
        public bool inner = true;
        public int placementQuantityMin;
        [Min(1)] public int placementQuantityMax;

        [Space, Header("Group placement:")] public bool placeAsGroup = false;
        public int groupMinCount;
        [Min(1)] public int groupMaxCount;
    }
}