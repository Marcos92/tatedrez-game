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

        GameEvents.StartTurn.AddListener(EnableInput);
        GameEvents.EndTurn.AddListener(DisableInput);

        GameEvents.StartTicTacToe.AddListener(DisableInput);
        GameEvents.StartChess.AddListener(DisableInput);
        GameEvents.EndGame.AddListener(DisableInput);
    }

    private bool CanMove()
    {
        bool isPlayerTurn = color == GameManager.Instance.CurrentPlayerColor;
        return isPlayerTurn && GameManager.Instance.MoveValidation.CanPieceMove(this);
    }

    private void StartMove()
    {
        pieceMovement.StartMove();
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

        GameEvents.PieceRelease.Invoke();
    }

    public void DropOnTile(Tile newTile)
    {
        if (tile)
        {
            tile.RemovePiece();
        }

        tile = newTile;

        pieceMovement.Drop(tile.transform);
    }

    private void ToggleInput(bool value)
    {
        pieceInput.ToggleInput(value);
        pieceRenderer.ToggleGlow(value);
    }

    private void EnableInput()
    {
        ToggleInput(CanMove());
    }

    private void DisableInput()
    {
        ToggleInput(false);
    }
}