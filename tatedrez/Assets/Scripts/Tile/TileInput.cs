using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public Action OnTileEnter;
    public Action OnTileExit;
    public Action<Piece> OnTileDrop;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnTileEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnTileExit.Invoke();
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnTileDrop.Invoke(eventData.pointerDrag.GetComponent<Piece>());
    }
}
