using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    private Piece piece;
    public Piece Piece { get => piece; }

    private TileInput tileInput;
    private TileRenderer tileRenderer;
    private TileAudio tileAudio;

    public static UnityEvent pieceDroppedEvent;

    void Awake()
    {
        tileInput = GetComponent<TileInput>();
        tileRenderer = GetComponent<TileRenderer>();
        tileAudio = GetComponent<TileAudio>();

        tileInput.OnTileEnter += TileEnter;
        tileInput.OnTileExit += TileExit;
        tileInput.OnTileDrop += DropPiece;
    }

    private void TileEnter()
    {
        if (!HasPiece())
        {
            tileRenderer.FadeInHover();
        }
    }

    private void TileExit()
    {
        tileRenderer.FadeOutHover();
    }

    public void Initialize(bool color)
    {
        tileRenderer.SetColor(color);
    }

    public void DropPiece(Piece piece)
    {
        if (HasPiece())
            return;

        this.piece = piece;
        this.piece.DropOnTile(this);

        tileAudio.PlayTileDropSound();

        GameEvents.PiecePlayed.Invoke();
    }

    public bool HasPiece()
    {
        return piece != null;
    }

    public bool HasPieceOfColor(PlayerColor color)
    {
        return HasPiece() && piece.Color == color;
    }

    public void RemovePiece()
    {
        piece = null;
    }

    public void ShowValidMoveHighlight()
    {
        tileRenderer.FadeInValidMove();
    }

    public void HideValidMoveHighlight()
    {
        tileRenderer.FadeOutValidMove();
    }
}