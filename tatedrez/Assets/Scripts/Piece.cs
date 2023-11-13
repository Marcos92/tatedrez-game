using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Type
    {
        ROOK,
        BISHOP,
        KNIGHT
    }
    public Type type;
    public int player;

    private Transform newParent;
    public Image image;

    public void OnBeginDrag(PointerEventData eventData)
    {
        newParent = transform.parent;

        //Places the piece above everything on the hierarchy
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(newParent);
        transform.localPosition = Vector2.zero;
        image.raycastTarget = true;
    }

    public void SetNewParent(Transform t)
    {
        newParent = t;
    }
}
