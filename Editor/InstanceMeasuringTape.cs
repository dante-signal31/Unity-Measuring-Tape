

using UnityEditor;
using UnityEngine;

public class InstanceMeasuringTape
{
    private static readonly string _prefabPath = 
        "Assets/Runtime/Prefabs/MeasuringTape.prefab"; 

    [MenuItem("Tools/Add measure tape")] 
    public static void InstanceTape()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabPath);
        
        if (prefab == null)
        {
            Debug.LogError($"Measuring tape prefab not found at: {_prefabPath}");
            return;
        }
        
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        if (instance == null) 
        {
            Debug.LogError($"Measuring tape was not instanced.");
            return;
        }
        
        // Select the instance in the hierarchy to make it visible.
        Selection.activeGameObject = instance;
    }
}
