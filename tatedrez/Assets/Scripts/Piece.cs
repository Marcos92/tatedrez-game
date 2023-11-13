using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Player
    {
        WHITE,
        BLACK
    }
    public Player player;

    public enum Type
    {
        ROOK,
        BISHOP,
        KNIGHT
    }
    public Type type;

    public Sprite[] sprites;

    private Transform newParent;
    public Image image;

    void Awake()
    {
        image.sprite = sprites[(int)type + (int)player * 3];
    }

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
        if (transform.parent.TryGetComponent<BoardTile>(out BoardTile tile))
        {
            if (newParent != transform.parent)
            {
                tile.RemovePiece();
            }
        }

        transform.SetParent(newParent);
        transform.localPosition = Vector2.zero;
        image.raycastTarget = true;
    }

    public void SetNewParent(Transform t)
    {
        newParent = t;
    }
}
