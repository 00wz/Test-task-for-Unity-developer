using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
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
        //List<string> collectableObjectsIDs = 
          StringArrayContainer cont =  
            await SaveHelper.LoadAsync<StringArrayContainer>(SAVE_KEY);
        //checking for missing or outdated data
        if (cont == null)
        {
            return;
        }
        if(cont.collectableObjectsIDs.Count != INVENTORY_CAPACITY)
        {
            Debug.LogWarning("Inventory: Incorrect loaded data");
            return;
        }

        for(int i = 0; i < INVENTORY_CAPACITY; i++)
        {
            if(cont.collectableObjectsIDs[i] == null)
            {
                CleanSlot(i);
                continue;
            }
            CollectedObjectConfig newCollectedObj = 
                CollectedObjectConfig.FindConfigByName(cont.collectableObjectsIDs[i]);
            if (newCollectedObj == null)
            {
                Debug.LogWarning($"Inventory: Missing config " +
                    $"by name: {cont.collectableObjectsIDs[i]}");
                CleanSlot(i);
                continue;
            }
            SetSlot(i, newCollectedObj);
        }
    }

    [Serializable]
    private class StringArrayContainer
    {
        public List<string> collectableObjectsIDs;
    }

    private async UniTask Save()
    {
        StringArrayContainer cont = new();
        //List<string> collectableObjectsIDs
         cont.collectableObjectsIDs = new List<string>(INVENTORY_CAPACITY);
        for (int i = 0; i < INVENTORY_CAPACITY; i++)
        {
            if (_inventory[i] != null)
            {
                cont.collectableObjectsIDs.Add(_inventory[i].Name);
            }
            else
            {
                cont.collectableObjectsIDs.Add(null);
            }
        }
        await SaveHelper.SaveAsync(cont, SAVE_KEY);
    }

    private void CleanSlot(int index)
    {
        _inventory[index] = null;
        _view.ClearSlot(index);
    }

    private void SetSlot(int index, CollectedObjectConfig config)
    {
        _inventory[index] = config;
        _view.SetSlot(index, config, null);
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
