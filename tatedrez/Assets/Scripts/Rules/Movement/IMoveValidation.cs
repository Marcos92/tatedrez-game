public interface IMoveValidation
{
    public bool CanPieceMove(Piece piece);
    public bool CanPlayerMove(PlayerColor playerColor);
}