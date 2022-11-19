using UnityEngine;

namespace HNW
{
    [System.Serializable]
    public struct MapStruct
    {
        public Vector2Int mapSize;
        [Range(0, 1)] public float obstaclePercent;
        public int seed;
        [Min(.1f)] public float minObstacleHeight;
        public float maxObstacleHeight;
        public Material tileMaterial;
        public Material obstacleMaterial;
        public Color bottomColor;
        public Color topColor;

        public Vector2Int MapCentre => new Vector2Int(mapSize.x / 2, mapSize.y / 2);
    }
}