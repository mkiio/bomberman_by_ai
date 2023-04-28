using UnityEngine;

public class GridInstantiator : MonoBehaviour
{
    public GameObject destructibleBlockPrefab; // The destructible block prefab object to be instantiated
    public GameObject indestructibleBlockPrefab; // The indestructible block prefab object to be instantiated
    public GameObject traversableBlockPrefab; // The traversable block prefab object to be instantiated
    public float spacing = 1.0f; // The spacing between each instantiated object
    public BlockType[,] map; // A 2D array representing the layout of the game map

    public void SetMap(BlockType[,] newMap)
    {
        map = newMap;
           

        int width = map.GetLength(0);
        int height = map.GetLength(1);

        // Delete existing blocks
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Spawn new blocks
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * spacing, 0.0f, y * spacing);
                GameObject prefab = null;

                switch (map[x, y])
                {
                    case BlockType.Destructible:
                        prefab = destructibleBlockPrefab;
                        break;
                    case BlockType.Indestructible:
                        prefab = indestructibleBlockPrefab;
                        break;
                    case BlockType.Traversable:
                        prefab = traversableBlockPrefab;
                        break;
                }

                if (prefab != null)
                {
                    GameObject block = Instantiate(prefab, position, Quaternion.identity);
                    block.transform.parent = transform;
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
        // Get the existing block at the specified position
        Transform existingBlock = transform.GetChild(x + y * map.GetLength(0));

        // Destroy the existing block and spawn the new block
        Vector3 spawnPosition = new Vector3(x * spacing, 0.0f, y * spacing);
        GameObject prefab = null;

        switch (newType)
        {
            case BlockType.Destructible:
                prefab = destructibleBlockPrefab;
                break;
            case BlockType.Indestructible:
                prefab = indestructibleBlockPrefab;
                break;
            case BlockType.Traversable:
                prefab = traversableBlockPrefab;
                break;
        }

        if (prefab != null)
        {
            GameObject newBlock = Instantiate(prefab, spawnPosition, Quaternion.identity);
            newBlock.transform.parent = transform;

            // Replace the existing block with the new block in the map array
            map[x, y] = newType;

            if (existingBlock != null)
            {
                Destroy(existingBlock.gameObject);
            }
        }
    }
}

}

public enum BlockType
{
    Destructible,
    Indestructible,
    Traversable
}
