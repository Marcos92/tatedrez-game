using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private PlayerColor color;
    public PlayerColor Color { get => color; }

    private Tile tile;
    public Tile Tile { get => tile; }

    private PieceInput pieceInput;
    private PieceMovement pieceMovement;
    private PieceRenderer pieceRenderer;
    private PieceAudio pieceAudio;

    void Awake()
    {
        pieceInput = GetComponent<PieceInput>();
        pieceMovement = GetComponent<PieceMovement>();
        pieceRenderer = GetComponent<PieceRenderer>();
        pieceAudio = GetComponent<PieceAudio>();

        pieceInput.OnPieceBeginDrag += StartMove;
        pieceInput.OnPieceDrag += Move;
        pieceInput.OnPieceEndDrag += EndMove;

        GameEvents.StartTurn.AddListener(UpdateState);
    }

    private void UpdateState()
    {
        ToggleInteraction(CanMove());
    }

    private bool CanMove()
    {
        bool isPlayerTurn = color == GameManager.Instance.CurrentPlayerColor;
        bool isValidForTictactoe = GameManager.Instance.CurrentPhase == GamePhase.TICTACTOE && !tile;
        bool isValidForChess = GameManager.Instance.CurrentPhase == GamePhase.CHESS && MoveValidation.CanPieceMove(this);
        return isPlayerTurn && (isValidForTictactoe || isValidForChess);
    }

    private void StartMove()
    {
        pieceMovement.StartMove();
        pieceRenderer.ToggleGlow(false);
        pieceRenderer.ToggleShadow(false);
        pieceAudio.PlayPieceSelectedAudio();

        GameEvents.PieceHold.Invoke(this);
    }

    private void Move()
    {
        pieceMovement.MoveTo(Input.mousePosition);
    }

    private void EndMove()
    {
        pieceMovement.EndMove();
        pieceRenderer.ToggleShadow(true);
        pieceRenderer.ToggleGlow(CanMove());

        GameEvents.PieceRelease.Invoke();
    }

    public void Drop(Tile tile)
    {
        this.tile = tile;
        pieceMovement.Drop(tile.transform);
    }

    private void ToggleInteraction(bool value)
    {
        pieceInput.enabled = value;
        pieceRenderer.ToggleGlow(value);
    }
}
