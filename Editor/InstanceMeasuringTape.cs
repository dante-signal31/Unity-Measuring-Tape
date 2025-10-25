

using UnityEditor;
using UnityEngine;

public class InstanceMeasuringTape
{
    // Although in the Project browser you see this package under
    // Packages/Unity Measuring Tape, actually the real name of the folder is the one
    // set name variable from Package.json, in my case ninja.dlab.unity-measuring-tape.
    private static readonly string _prefabPath = 
        "Packages/ninja.dlab.unity-measuring-tape/Runtime/Prefabs/MeasuringTape.prefab"; 

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
