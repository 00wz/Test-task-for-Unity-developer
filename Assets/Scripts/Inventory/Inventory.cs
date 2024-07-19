using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public const int INVENTORY_HEIGHT = 2;
    public const int INVENTORY_WIDTH = 3;
    public const int INVENTORY_CAPACITY = INVENTORY_HEIGHT * INVENTORY_WIDTH;
    private const string INVENTORY_VIEW_PATH = "InventoryView";

    private InventoryView _view;
    private CollectedObjectConfig[] _inventory;

    public Inventory()
    {
        _inventory = new CollectedObjectConfig[INVENTORY_CAPACITY];
        var viewResource = Resources.Load<InventoryView>(INVENTORY_VIEW_PATH);
        var canvas = GameObject.FindObjectOfType<Canvas>();
        _view = GameObject.Instantiate(viewResource, canvas.transform);
        _view.Init(INVENTORY_HEIGHT, INVENTORY_WIDTH);
    }

    public bool TryAdd(CollectedObjectConfig config)
    {
        int newIndex = GetFirstEmptySlotIndex();
        if(newIndex == -1)
        {
            return false;
        }
        _inventory[newIndex] = config;
        _view.SetSlot(newIndex, config, null);
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
