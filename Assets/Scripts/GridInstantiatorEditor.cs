using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridInstantiator))]
public class GridInstantiatorEditor : Editor
{
    private BlockType[,] map;
    private SerializedProperty blockPrefabsProp;
    private SerializedProperty spacingProp;

    private void OnEnable()
    {
        blockPrefabsProp = serializedObject.FindProperty("blockPrefabs");
        spacingProp = serializedObject.FindProperty("spacing");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        int width = 13;
        int height = 11;

        if (map == null || map.GetLength(0) != width || map.GetLength(1) != height)
        {
            map = new BlockType[width, height];
        }

        EditorGUILayout.LabelField("Map Layout");

        for (int y = height - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < width; x++)
            {
                map[x, y] = (BlockType)EditorGUILayout.EnumPopup(map[x, y], GUILayout.Width(60.0f), GUILayout.Height(20.0f));
            }

            EditorGUILayout.EndHorizontal();
        }

        GridInstantiator myScript = (GridInstantiator)target;

        if (GUILayout.Button("Apply Map"))
        {
            myScript.SetMap(map);
        }

        if (GUILayout.Button("Apply Default Map"))
        {
            map = myScript.CreateDefaultMap();
            myScript.SetMap(map);
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(blockPrefabsProp, true);
        EditorGUILayout.PropertyField(spacingProp);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        GridInstantiator gridInstantiator = (GridInstantiator)target;
        gridInstantiator.map = map;
    }

private BlockType[,] CreateMiddleTraversableMap()
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
