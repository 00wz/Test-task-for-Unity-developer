using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectedObjectConfig", menuName = "Configs/CollectedObjectConfig")]
public class CollectedObjectConfig : ScriptableObject
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private GameObject _sceneView;
    public GameObject SceneView => _sceneView;

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField]
    private string _tooltip;
    public string Tooltip => _tooltip;

    static WeakReference _configsListSoftReference = new WeakReference(null, false);
    /// <summary>
    /// Returns the first found config with name or null in case of failure
    /// Uses a weak reference to list all configs for caching
    /// </summary>
    /// <param name="name">name of CollectedObject</param>
    /// <returns></returns>
    static public CollectedObjectConfig FindConfigByName(string name)
    {
        CollectedObjectConfig[] configList = 
            _configsListSoftReference.Target as CollectedObjectConfig[];
        
        if(configList == null)
        {
            configList =
                Resources.FindObjectsOfTypeAll<CollectedObjectConfig>();
            _configsListSoftReference.Target = configList;
        }
        
        return configList.FirstOrDefault(config => { return config.Name == name; });
    }
}
