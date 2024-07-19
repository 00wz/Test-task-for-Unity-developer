using System;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform DropTargetPosition;

    public event Func<CollectedObjectConfig, bool> OnCollect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<CollectedObject>(
            out CollectedObject collectedObject))
        {
            if (OnCollect != null && OnCollect.Invoke(collectedObject.Config))
            {
                collectedObject.OnCollect();
            }
        }
    }

    public void DropCollectedObject(CollectedObjectConfig config)
    {
        CollectedObjectFactory.Instantiate(config, DropTargetPosition.position);
    }
}
