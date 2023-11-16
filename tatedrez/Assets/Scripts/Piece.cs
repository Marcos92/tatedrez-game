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

    [Header("Visuals")]
    public Image image;
    public GameObject glow;
    public GameObject shadow;

    private AudioSource audioSource;

    void Start()
    {
        image.raycastTarget = false;

        audioSource = GetComponent<AudioSource>();

        BoardManager.Instance.startGameEvent.AddListener(UpdateState);
        BoardManager.Instance.endTurnEvent.AddListener(UpdateState);
        BoardManager.Instance.endGameEvent.AddListener(Disable);
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

        audioSource.Play();

        shadow.SetActive(false);

        Disable();
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
                tile.FadeOutAllHighlights();
            }

            transform.SetParent(newParent);
            transform.localPosition = Vector2.zero;

            BoardManager.Instance.EndTurn();
        }
        else
        {
            transform.SetParent(oldParent);
            transform.localPosition = Vector2.zero;

            Enable();
        }

        shadow.SetActive(true);

        BoardManager.Instance.ResetValidMoves();
    }

    public void SetNewParent(Transform t)
    {
        newParent = t;
    }

    private void UpdateState()
    {
        bool isPlayerTurn = playerColor == BoardManager.Instance.currentPlayerColor;
        bool isValidForTictactoe = BoardManager.Instance.currentPhase == BoardManager.Phase.TICTACTOE && !transform.parent.GetComponent<BoardTile>();
        bool isValidForChess = BoardManager.Instance.currentPhase == BoardManager.Phase.CHESS && BoardManager.Instance.CheckIfPieceHasValidMoves(transform.parent.GetComponent<BoardTile>());

        bool interactable = isPlayerTurn && (isValidForTictactoe || isValidForChess);
        image.raycastTarget = interactable;
        glow.SetActive(interactable && BoardManager.Instance.currentPlayerColor == playerColor);
    }

    private void Enable()
    {
        image.raycastTarget = true;
        glow.SetActive(true);
    }

    private void Disable()
    {
        image.raycastTarget = false;
        glow.SetActive(false);
    }
}
