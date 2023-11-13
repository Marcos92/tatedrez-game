using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BoardManager.Player player;

    public enum Type
    {
        ROOK,
        BISHOP,
        KNIGHT
    }
    public Type type;

    public Sprite[] sprites;

    private Transform oldParent;
    private Transform newParent;

    public Image image;

    void Start()
    {
        image.sprite = sprites[(int)type + (int)player * 3];
        image.raycastTarget = false;

        BoardManager.Instance.endTurnEvent.AddListener(UpdateState);
        UpdateState();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = transform.parent;
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

        if (oldParent != newParent)
        {
            BoardManager.Instance.EndTurn();

            if (oldParent.TryGetComponent<BoardTile>(out BoardTile tile))
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

    private void UpdateState()
    {
        image.raycastTarget = player == BoardManager.Instance.playerTurn;
    }
}
