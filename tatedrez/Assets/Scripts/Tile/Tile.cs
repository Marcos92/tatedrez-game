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
        this.piece.Drop(this);

        tileAudio.PlayTileDropSound();

        GameEvents.PiecePlayed.Invoke();
    }

    public bool HasPiece()
    {
        return piece != null;
    }

    public void RemovePiece()
    {
        piece = null;
    }

    public void ToggleInteraction(bool value)
    {
        tileInput.enabled = value;
    }

    public void ShowValidMoveHighlight()
    {
        tileRenderer.FadeInValidMove();
    }
}