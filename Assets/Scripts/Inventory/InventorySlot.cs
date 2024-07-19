using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private float animDuration;

    private event Action _onClick;
    private event Action _onPointing;
    private event Action _onUnpointing;

    private float _originSize;
    private Transform _transform;
    private Image _icon;
    private Sprite _defaultIcon;
    public CollectedObjectConfig _config { get; private set; }

    private void Awake()
    {
        _originSize = transform.localScale.x;
        _transform = transform;
        _icon = GetComponent<Image>();
        _defaultIcon = _icon.sprite;
    }

    public void Clear()
    {
        _config = null;
        _icon.sprite = _defaultIcon;
        _onClick = null;
        _onPointing = null;
        _onUnpointing = null;
    }

    public void Set(CollectedObjectConfig config, Action onClick,
        Action onPointing, Action onUnpointing)
    {
        _config = config;
        _icon.sprite = config.Icon;
        _onClick = onClick;
        _onPointing = onPointing;
        _onUnpointing = onUnpointing;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _transform.DOScale(_originSize - 0.1f, animDuration);
        _onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _transform.DOScale(_originSize + 0.1f, animDuration);
        _onPointing?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _transform.DOScale(_originSize, animDuration);
        _onUnpointing?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _transform.DOScale(_originSize, animDuration);
    }
}
