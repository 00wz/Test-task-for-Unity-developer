using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Inventory
{
    public const int INVENTORY_HEIGHT = 2;
    public const int INVENTORY_WIDTH = 3;
    public const int INVENTORY_CAPACITY = INVENTORY_HEIGHT * INVENTORY_WIDTH;
    private const string INVENTORY_VIEW_PATH = "InventoryView";
    private const string SAVE_KEY = "InventoryData";

    private InventoryView _view;
    private CollectedObjectConfig[] _inventory;
    public event Action<CollectedObjectConfig> OnDrop;

    public Inventory()
    {
        _inventory = new CollectedObjectConfig[INVENTORY_CAPACITY];
        LoadAndApply();
        var viewResource = Resources.Load<InventoryView>(INVENTORY_VIEW_PATH);
        var canvas = GameObject.FindObjectOfType<Canvas>();
        _view = GameObject.Instantiate(viewResource, canvas.transform);
        _view.Init(INVENTORY_HEIGHT, INVENTORY_WIDTH);
    }

    private async UniTask LoadAndApply()
    {
        string[] collectableObjectsIDs =  
            await SaveHelper.LoadAsync<string[]>(SAVE_KEY);

        //checking for missing or outdated data
        if (collectableObjectsIDs == null)
        {
            return;
        }
        if(collectableObjectsIDs.Length != INVENTORY_CAPACITY)
        {
            Debug.LogWarning("Inventory: Incorrect loaded data");
            return;
        }

        for(int i = 0; i < INVENTORY_CAPACITY; i++)
        {
            if(collectableObjectsIDs[i] == null)
            {
                CleanSlot(i);
                continue;
            }

            CollectedObjectConfig newCollectedObj = 
                CollectedObjectConfig.FindConfigByName(collectableObjectsIDs[i]);
            if (newCollectedObj == null)
            {
                Debug.LogWarning($"Inventory: Missing config " +
                    $"by name: {collectableObjectsIDs[i]}");
                CleanSlot(i);
                continue;
            }
            SetSlot(i, newCollectedObj);
        }
    }

    private async UniTask Save()
    {
        string[] collectableObjectsIDs = new string[INVENTORY_CAPACITY];
        for (int i = 0; i < INVENTORY_CAPACITY; i++)
        {
            if (_inventory[i] != null)
            {
                collectableObjectsIDs[i] = (_inventory[i].Name);
            }
        }
        await SaveHelper.SaveAsync(collectableObjectsIDs, SAVE_KEY);
    }

    private void CleanSlot(int index)
    {
        _inventory[index] = null;
        _view.ClearSlot(index);
    }

    private void SetSlot(int index, CollectedObjectConfig config)
    {
        _inventory[index] = config;
        _view.SetSlot(index, config, () => Drop(index));
    }

    private void Drop(int index)
    {
        OnDrop?.Invoke(_inventory[index]);
        CleanSlot(index);
        Save();
    }

    public bool TryAdd(CollectedObjectConfig config)
    {
        int newIndex = GetFirstEmptySlotIndex();
        if(newIndex == -1)
        {
            return false;
        }
        SetSlot(newIndex, config);
        Save();
        return true;
    }

    private int GetFirstEmptySlotIndex()
    {
        for(int i = 0; i < INVENTORY_CAPACITY; i++)
        {
            if(_inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void ToggleViewVisibility()
    {
        _view.ToggleVisibility();
    }
}
