using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedObject : MonoBehaviour
{
    [SerializeField]
    private CollectedObjectConfig Config;

    void Start()
    {
        Instantiate(Config.SceneView, transform.position, transform.rotation, transform);
    }

    public void OnCollect()
    {
        //collect animation
        Destroy(this.gameObject);
    }
}
