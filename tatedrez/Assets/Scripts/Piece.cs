using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BoardManager.PlayerColor playerColor;

    public enum Type
    {
        ROOK,
        BISHOP,
        KNIGHT
    }
    public Type type;

    private Transform oldParent;
    private Transform newParent;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.raycastTarget = false;

        BoardManager.Instance.endTurnEvent.AddListener(UpdateState);
        BoardManager.Instance.endGameEvent.AddListener(Disable);
        UpdateState();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = transform.parent;
        newParent = transform.parent;

        if (transform.parent.TryGetComponent<BoardTile>(out BoardTile tile))
        {
            BoardManager.Instance.CheckIfPieceHasValidMoves(tile, true);
        }

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
            if (oldParent.TryGetComponent<BoardTile>(out BoardTile tile))
            {
                tile.RemovePiece();
            }

            transform.SetParent(newParent);
            transform.localPosition = Vector2.zero;

            BoardManager.Instance.EndTurn();
        }
        else
        {
            transform.SetParent(oldParent);
            transform.localPosition = Vector2.zero;
            image.raycastTarget = true;
        }

        BoardManager.Instance.ResetValidMoves();
    }

    public void SetNewParent(Transform t)
    {
        newParent = t;
    }

    private void UpdateState()
    {
        bool isPlayerTurn = playerColor == BoardManager.Instance.currentPlayerColor;
        bool isValidForTictactoe = !transform.parent.GetComponent<BoardTile>() && BoardManager.Instance.currentPhase == BoardManager.Phase.TICTACTOE;
        bool isValidForChess = BoardManager.Instance.currentPhase == BoardManager.Phase.CHESS;

        image.raycastTarget = isPlayerTurn && (isValidForTictactoe || isValidForChess);
    }

    private void Disable()
    {
        image.raycastTarget = false;
    }
}
