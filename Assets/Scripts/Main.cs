using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private MainCharacter Character;

    private Inventory _inventory;
    private CancellationTokenSource _tokenSource = new();

    private async UniTask UpdateAsync(CancellationToken cancellationToken)
    {
        while(!cancellationToken.IsCancellationRequested)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log(typeof(string[,]).IsSerializable);
                //CollectedObjectConfig.FindConfigByName(" ");
                _inventory.ToggleViewVisibility();
            }

            await UniTask.Yield(cancellationToken);
        }
    }

    private void Start()
    {
        _inventory = new Inventory();
        Character.OnCollect += _inventory.TryAdd;
        _inventory.OnDrop += Character.DropCollectedObject;
        UpdateAsync(_tokenSource.Token);
    }

    private void OnDestroy()
    {
        _tokenSource.Cancel();
    }
}
