using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private InventorySlot inventorySlotPrefab;
    [SerializeField]
    private float inventoryHeightPx = 230f;
    [SerializeField]
    private float animDuration = 0.1f;

    private GridLayoutGroup _grid;
    private InventorySlot[] _slots = null;
    private Vector3 _visiblePosition;
    private Vector3 _invisiblePosition;
    public bool IsVisible { get; private set; } = true;

    void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _visiblePosition = transform.position;
        _invisiblePosition = _visiblePosition + Vector3.up * inventoryHeightPx;
    }

    public void Init(int haight, int width)
    {
        Clean();
        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = width;
        int slotsCount = haight * width;
        _slots = new InventorySlot[slotsCount];
        for(int i = 0; i < slotsCount; i++)
        {
            _slots[i] = Instantiate(inventorySlotPrefab, transform);
        }
    }

    public void ToggleVisibility()
    {
        if (IsVisible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {
        transform.DOKill();
        IsVisible = true;
        transform.DOMove(_visiblePosition, animDuration);
    }

    public void Hide()
    {
        transform.DOKill();
        IsVisible = false;
        transform.DOMove(_invisiblePosition, animDuration);
    }

    public void SetSlot(int index, CollectedObjectConfig config, Action OnClick)
    {
        _slots[index].Set(config, OnClick);
    }

    private void Clean()
    {
        if(_slots == null)
        {
            return;
        }
        for(int i = 0; i < _slots.Length; i++)
        {
            Destroy(_slots[i].gameObject);
        }
        _slots = null;
    }

    private void OnDestroy()
    {
        Clean();
    }
}
