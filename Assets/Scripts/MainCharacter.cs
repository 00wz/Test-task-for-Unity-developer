using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<CollectedObject>(
            out CollectedObject collectedObject))
        {
            collectedObject.OnCollect();
        }
    }
}
