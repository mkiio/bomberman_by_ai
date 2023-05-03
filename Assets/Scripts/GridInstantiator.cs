using UnityEngine;

public class GridInstantiator : MonoBehaviour
{
    public GameObject[] blockPrefabs; // The block prefabs to be instantiated
    public float spacing = 1.0f; // The spacing between each instantiated object
    public bool useDefaultMap = false;
    public BlockType[,] map; // A 2D array representing the layout of the game map

    private GameObject[,] blocks; // A 2D array representing the instantiated blocks
    public static GridInstantiator instance; // The static instance of the GridInstantiator

    private void Awake()
    {
        // Set the static instance to this object
        instance = this;
        if (useDefaultMap) SetMap(CreateDefaultMap());
    }

    public void SetMap(BlockType[,] newMap)
    {
        map = newMap;
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        // Destroy existing blocks
        if (blocks != null)
        {
            foreach (GameObject block in blocks)
            {
                if (block != null)
                {
                    Destroy(block);
                }
            }
        }

        // Instantiate new blocks
        blocks = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * spacing, 0.0f, y * spacing);
                int prefabIndex = (int)map[x, y];
                if (prefabIndex >= 0 && prefabIndex < blockPrefabs.Length)
                {
                    GameObject prefab = blockPrefabs[prefabIndex];
                    if (prefab != null)
                    {
                        GameObject block = Instantiate(prefab, position, Quaternion.identity, transform);
                        blocks[x, y] = block;
                    }
                }
            }
        }
    }

    public void SwapBlock(Vector3 position, BlockType newType)
    {
        int x = Mathf.RoundToInt(position.x / spacing);
        int y = Mathf.RoundToInt(position.z / spacing);

        if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
        {
            // Destroy existing block and instantiate new block
            int prefabIndex = (int)newType;
            if (prefabIndex >= 0 && prefabIndex < blockPrefabs.Length)
            {
                GameObject prefab = blockPrefabs[prefabIndex];
                if (prefab != null)
                {
                    if (blocks[x, y] != null)
                    {
                        Destroy(blocks[x, y]);
                    }
                    Vector3 spawnPosition = new Vector3(x * spacing, 0.0f, y * spacing);
                    GameObject newBlock = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
                    blocks[x, y] = newBlock;
                    map[x, y] = newType;
                }
            }
        }
    }

    public BlockType[,] CreateDefaultMap()
    {
        int width = 13;
        int height = 11;
        BlockType[,] map = new BlockType[width, height];

        // Set the outside edges to indestructible
        for (int x = 0; x < width; x++)
        {
            map[x, 0] = BlockType.Indestructible;
            map[x, height - 1] = BlockType.Indestructible;
        }

        for (int y = 1; y < height - 1; y++)
        {
            map[0, y] = BlockType.Indestructible;
            map[width - 1, y] = BlockType.Indestructible;
        }

        // Set the middle to a pattern of traversable and indestructible blocks
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (x % 2 == 1 && y % 2 == 1)
                {
                    // Set indestructible block
                    map[x, y] = BlockType.Indestructible;

                    // Set surrounding traversable blocks
                    map[x - 1, y - 1] = BlockType.Traversable;
                    map[x, y - 1] = BlockType.Traversable;
                    map[x + 1, y - 1] = BlockType.Traversable;
                    map[x - 1, y] = BlockType.Traversable;
                    map[x + 1, y] = BlockType.Traversable;
                    map[x - 1, y + 1] = BlockType.Traversable;
                    map[x, y + 1] = BlockType.Traversable;
                    map[x + 1, y + 1] = BlockType.Traversable;
                }
                else
                {
                    // Set traversable block
                    map[x, y] = BlockType.Traversable;
                }
            }
        }

        return map;
    }
}

public enum BlockType
{
    Destructible,
    Indestructible,
    Traversable
}
