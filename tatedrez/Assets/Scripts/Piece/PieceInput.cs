using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;

    public Action OnPieceBeginDrag;
    public Action OnPieceDrag;
    public Action OnPieceEndDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnPieceBeginDrag.Invoke();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnPieceDrag.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnPieceEndDrag.Invoke();
        image.raycastTarget = true;
    }
}