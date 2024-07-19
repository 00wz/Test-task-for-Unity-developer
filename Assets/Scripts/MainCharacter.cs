using System;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
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
}
