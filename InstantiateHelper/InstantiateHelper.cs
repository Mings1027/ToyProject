using UnityEngine;

public static class InstantiateHelper
{
    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Debug.Log($"Instantiate {prefab.name} at {System.Environment.StackTrace}");
        return Object.Instantiate(prefab, position, rotation);
    }

    public static GameObject Instantiate(GameObject prefab)
    {
        Debug.Log($"Instantiate {prefab.name} at {System.Environment.StackTrace}");
        return Object.Instantiate(prefab);
    }
}