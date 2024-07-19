using UnityEngine;

public static class CollectedObjectFactory
{
    private const string COLLECTED_OBJECT_PREFAB_PATH = "CollectableObject";

    public static CollectedObject Instantiate(CollectedObjectConfig config, Vector3 position)
    {
        var tmp = Resources.Load<CollectedObject>(COLLECTED_OBJECT_PREFAB_PATH);
        tmp.Config = config;
        return GameObject.Instantiate(tmp, position, Quaternion.identity);
    }
}
