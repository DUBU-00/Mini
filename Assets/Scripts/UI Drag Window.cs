using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform windowTransform;

    private Canvas _canvas;

    void Start()
    {
        _canvas = GetComponentInParent<Canvas>();

        if (windowTransform == null )
        {
            windowTransform = GetComponent<RectTransform>();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        windowTransform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null)
            return;
        windowTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
}
