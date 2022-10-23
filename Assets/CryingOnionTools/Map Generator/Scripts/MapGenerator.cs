using HNW;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Map[] maps;
    [SerializeField] int mapIndex;
    [SerializeField] Transform tilePref;
    [SerializeField] Transform obstaclePref;
    [SerializeField] LayerMask navMeshLayer;
    [SerializeField] float tileSize = 1;
    [SerializeField, Range(0f, 1f)] float outlinePercent;

    Queue<Vector2Int> shuffleTileCoords;
    Queue<Vector3> shufflePositions;

    Map currentMap;
    BoxCollider boxCollider;

    private void Awake()
    {
        FindObjectOfType<Spawner>().OnNextWave += NewWave;
    }

    private void OnDestroy()
    {
        FindObjectOfType<Spawner>().OnNextWave -= NewWave;
    }

    void NewWave(int waveNumber)
    {
        mapIndex = Mathf.Clamp(waveNumber - 1, 0, maps.Length);
        StartCoroutine(GenerateMapRoutine());
    }

    private IEnumerator GenerateMapRoutine()
    {
        NavMesh.RemoveAllNavMeshData();

        yield return new WaitForEndOfFrame();

        GenerateMap();
    }



    public void GenerateMap()
    {
        if(!TryGetComponent(out boxCollider))
            boxCollider = gameObject.AddComponent<BoxCollider>();


        currentMap = maps[mapIndex];

        boxCollider.isTrigger = true;

        boxCollider.size = new Vector3(
                currentMap.mapSize.x * tileSize,
                (Mathf.Min(currentMap.mapSize.x, currentMap.mapSize.y) * tileSize) / 2f,
                currentMap.mapSize.y * tileSize
            );

        boxCollider.center = new Vector3(0, boxCollider.size.y / 2f, 0);

        System.Random rand = new System.Random(currentMap.seed);

        // Generating coords
        List<Vector2Int> allTileCoords = new List<Vector2Int>();

        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTileCoords.Add(new Vector2Int(x, y));
            }
        }

        shuffleTileCoords = new Queue<Vector2Int>(Utility.ShuffleArray(allTileCoords.ToArray(), currentMap.seed));

        // Create holders
        string holderName = "Generated Map";

        if (transform.Find(holderName))
        {
            if(Application.isEditor)
            {
                DestroyImmediate(transform.Find(holderName).gameObject);
            }
            else
            {
                Destroy(transform.Find(holderName).gameObject);
            }
        }
            

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        Transform tilesHolder = new GameObject("Tiles").transform;
        tilesHolder.parent = mapHolder;

        Transform obstaclesHolder = new GameObject("Obstacles").transform;
        obstaclesHolder.parent = mapHolder;

        // Spawning Obstacles
        bool[,] obstacleMap = new bool[currentMap.mapSize.x, currentMap.mapSize.y];

        int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector2Int randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;
            if (randomCoord != currentMap.MapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)rand.NextDouble());
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePref, obstaclePosition + Vector3.up * obstacleHeight / 2, Quaternion.identity, obstaclesHolder);
                newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);

                if(newObstacle.TryGetComponent(out Renderer obstacleRend))
                {
                    Material obstMaterial = new Material(currentMap.obstacleMaterial);
                    //float colourPercent = randomCoord.y / (float)currentMap.mapSize.y;
                    float colourPercent = newObstacle.localScale.y / (float)currentMap.maxObstacleHeight;
                    obstMaterial.color = currentMap.colorGradient.Evaluate(colourPercent);
                    obstacleRend.sharedMaterial = obstMaterial;
                }

                if(newObstacle.TryGetComponent(out NavMeshObstacle meshObstacle))
                    meshObstacle.carving = true;

            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        // Spawning tiles
        List<Vector3> positions = new List<Vector3>();

        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                if(obstacleMap[x, y] == false)
                {
                    Vector3 tilePosition = CoordToPosition(x, y);
                    Transform newTile = Instantiate(tilePref, tilePosition, Quaternion.Euler(Vector3.right * 90), tilesHolder);
                    newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                    positions.Add(newTile.position);
                }
            }
        }

        shufflePositions = new Queue<Vector3>(Utility.ShuffleArray(positions.ToArray(), currentMap.seed));

        // Combine tiles
        CombineMesh(tilesHolder.gameObject, mapHolder.gameObject, currentMap.tileMaterial);
    }

    private void CombineMesh(GameObject container, GameObject target, Material material)
    {
        MeshFilter[] meshFilters = container.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();
        NavMeshSurface navMeshSurface = target.AddComponent<NavMeshSurface>();
        navMeshSurface.layerMask = navMeshLayer;
        meshRenderer.sharedMaterial = material;

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
        navMeshSurface.BuildNavMesh();

        if (Application.isEditor)
            DestroyImmediate(container);
        else
            Destroy(container);
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(currentMap.MapCentre);

        mapFlags[currentMap.MapCentre.x, currentMap.MapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Vector2Int tile = queue.Dequeue();

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
                                queue.Enqueue(new Vector2Int(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return (new Vector3(-currentMap.mapSize.x / 2f + .5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize) + transform.position;
    }

    public Vector2Int GetRandomCoord()
    {
        Vector2Int randomCoord = shuffleTileCoords.Dequeue();
        shuffleTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Vector3 GetRandomPos()
    {
        Vector3 randomPos = shufflePositions.Dequeue();
        shufflePositions.Enqueue(randomPos);
        return randomPos;
    }

    public Vector3 GetMapCentrePos()
    {
        return CoordToPosition(currentMap.MapCentre.x, currentMap.MapCentre.y) + Vector3.up;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Damageable damageable))
            damageable.Health -= damageable.Health;
    }
}

[System.Serializable]
public class Map
{
    public Vector2Int mapSize = Vector2Int.one * 10;
    [Range(0, 1)] public float obstaclePercent = .25f;
    public int seed = "CryingOnion".GetHashCode();
    public float minObstacleHeight = 1;
    public float maxObstacleHeight = 3;
    public Material tileMaterial;
    public Material obstacleMaterial;
    public Gradient colorGradient;

    public Vector2Int MapCentre => new Vector2Int(mapSize.x / 2, mapSize.y / 2);
}