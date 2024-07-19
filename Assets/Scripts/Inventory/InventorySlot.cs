using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float animDuration;

    private event Action _onClick;

    private float _originSize;
    private Transform _transform;
    private Image _icon;
    private Sprite _defaultIcon;

    private void Awake()
    {
        _originSize = transform.localScale.x;
        _transform = transform;
        _icon = GetComponent<Image>();
        _defaultIcon = _icon.sprite;
    }

    public void Set(CollectedObjectConfig config, Action onClick)
    {
        _onClick = null;

        if(config == null)
        {
            _icon.sprite = _defaultIcon;
            return;
        }

        _icon.sprite = config.Icon;
        _onClick = onClick;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _transform.DOScale(_originSize - 0.1f, animDuration);
        _onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _transform.DOScale(_originSize + 0.1f, animDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _transform.DOScale(_originSize, animDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _transform.DOScale(_originSize, animDuration);
    }
}
