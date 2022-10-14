using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Material floorMaterial;
    [SerializeField] Transform tilePref;
    [SerializeField] Transform obstaclePref;
    [SerializeField] Vector2Int mapSize = Vector2Int.one * 10;
    [SerializeField] float tileScale = 1;
    [SerializeField, Range(0f, 1f)] float outlinePercent;
    [SerializeField, Range(0f, 1f)] float obstaclePercent;
    [SerializeField] int seed = "Lobinux".GetHashCode();

    Coord mapCentre;
    List<Coord> allTileCoords;
    Queue<Coord> shuffleTileCoords;


    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        shuffleTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));

        mapCentre = new Coord(mapSize.x / 2, mapSize.y / 2);

        string holderName = "Generated Map";

        if (transform.Find(holderName))
            DestroyImmediate(transform.Find(holderName).gameObject);

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        Transform tilesHolder = new GameObject("Tiles").transform;
        tilesHolder.parent = mapHolder;

        Transform obstaclesHolder = new GameObject("Obstacles").transform;
        obstaclesHolder.parent = mapHolder;

        bool[,] obstacleMap = new bool[mapSize.x, mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;
            if (randomCoord != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePref, obstaclePosition + Vector3.up / 2, Quaternion.identity, obstaclesHolder);
                newObstacle.localScale = Vector3.right * tileScale + Vector3.forward * tileScale + Vector3.up;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if(obstacleMap[x, y] == false)
                {
                    Vector3 tilePosition = CoordToPosition(x, y);
                    Transform newTile = Instantiate(tilePref, tilePosition, Quaternion.Euler(Vector3.right * 90), tilesHolder);
                    newTile.localScale = Vector3.one * tileScale * (1 - outlinePercent);
                }
            }
        }

        CombineMesh(tilesHolder.gameObject, mapHolder.gameObject);
    }

    private void CombineMesh(GameObject container, GameObject target)
    {
        MeshFilter[] meshFilters = container.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();
        meshRenderer.material = floorMaterial;

        MeshCollider meshCollider = target.AddComponent<MeshCollider>();

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh.CombineMeshes(combine);
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        DestroyImmediate(container);
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);

        mapFlags[mapCentre.x, mapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;

                    if(x == 0 || y == 0)
                    {
                        if(neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-(mapSize.x * tileScale) / 2 + (tileScale / 2) + (x * tileScale), 0, -(mapSize.y * tileScale) / 2 + (tileScale / 2) + (y * tileScale));
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffleTileCoords.Dequeue();
        shuffleTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Coord lhs, Coord rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

        public static bool operator !=(Coord lhs, Coord rhs) => !(lhs == rhs);
    }
}