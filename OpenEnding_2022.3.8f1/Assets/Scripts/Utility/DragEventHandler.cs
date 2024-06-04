using UnityEngine;
using UnityEngine.EventSystems;

public class DragEventHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool dragable = true;
    
    public delegate void DragHandler(PointerEventData eventData);
    public event DragHandler OnWhileDrag;
    public event DragHandler OnDragEnd;
    
    public void OnBeginDrag(PointerEventData eventData) {}
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragable) return;
        
        OnDragEnd?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragable) return;
        
        OnWhileDrag?.Invoke(eventData);
    }
}

    