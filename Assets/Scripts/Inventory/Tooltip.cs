using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    [SerializeField]
    private float animDuration;

    private RectTransform _maskRect;
    private CancellationTokenSource _tokenSource;

    private void Awake()
    {
        _maskRect = GetComponent<RectTransform>();
    }

    public async UniTask Show(string text)
    {
        tooltipText.text = text;
        //wait one frame for the ContentSizeFitter to calculate the size of the rectangle.
        await UniTask.Yield();
        StartCursorFallowing();
        DOTween.To(() => _maskRect.rect,
                    newRect =>
                    {
                        _maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newRect.width);
                        _maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newRect.height);
                    },
                    tooltipRect.rect,
                    animDuration);
    }

    public void Hide()
    {
        StopCursorFallowing();
        DOTween.To(() => _maskRect.rect,
            newRect =>
            {
                _maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newRect.width);
                _maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newRect.height);
            },
            Rect.zero,
            animDuration);
    }

    public void HideImmediately()
    {
        _maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        _maskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
    }

    private void StartCursorFallowing()
    {
        //return if the cursor is already in pursuit
        if (_tokenSource != null)
        {
            return;
        }

        _tokenSource = new();
        FollowCursor(_tokenSource.Token);
    }

    private async UniTask FollowCursor(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            transform.position = Input.mousePosition;
            await UniTask.Yield(cancellationToken);
        }
    }

    private void StopCursorFallowing()
    {
        if (_tokenSource != null && !_tokenSource.Token.IsCancellationRequested)
        {
            _tokenSource.Cancel();
            _tokenSource = null;
        }
    }
}
