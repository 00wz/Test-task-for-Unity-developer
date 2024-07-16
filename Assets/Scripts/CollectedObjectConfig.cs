using System.Collections;
using System.Collections.Generic;
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
}
